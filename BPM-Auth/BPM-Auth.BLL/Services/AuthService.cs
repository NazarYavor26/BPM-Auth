using BPM_Auth.BLL.Models;
using BPM_Auth.BLL.Models.User;
using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Entities;
using BPM_Auth.DAL.Enums;
using BPM_Auth.DAL.Repositories;
using BPM_Auth.ServiceBus.Models;
using BPM_Auth.ServiceBus.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BPM_Auth.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IPublisherService _publisherService;
        private readonly AppDbContext _db;

        public AuthService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IPublisherService publisherService,
            AppDbContext db)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _publisherService = publisherService;
            _db = db;
        }

        public string RegisterAdmin(AdminRegisterModel adminRegisterModel)
        {
            CreatePasswordHash(adminRegisterModel.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = adminRegisterModel.Email,
                Role = Role.Admin,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            _userRepository.Add(user);

            _publisherService.PublishAdminUserToBpmCore(new BpmCoreUserModel
            {
                UserId = user.UserId,
                FirstName = adminRegisterModel.FirstName,
                LastName = adminRegisterModel.LastName,
                Position = adminRegisterModel.Position,
                Email = user.Email,
                Role = user.Role
            });

            return user.UserId.ToString();
        }

        public string RegisterMember(MemberRegisterModel memberRegisterModel)
        {
            var supervisor = _userRepository.GetById(memberRegisterModel.SupervisorId)
                ?? throw new Exception($"Supervisor with Id {memberRegisterModel.SupervisorId} not found");

            CreatePasswordHash(memberRegisterModel.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = memberRegisterModel.Email,
                Role = memberRegisterModel.Role,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            _userRepository.Add(user);

            _publisherService.PublishMemberUserToBpmCore(new BpmCoreUserModel
            {
                UserId = user.UserId,
                FirstName = memberRegisterModel.FirstName,
                LastName = memberRegisterModel.LastName,
                Position = memberRegisterModel.Position,
                Email = user.Email,
                Role = user.Role,
                TeamId = memberRegisterModel.TeamId,
                SupervisorId = memberRegisterModel.SupervisorId
            });

            return user.UserId.ToString();
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string Login(UserLoginModel userLoginModel, HttpResponse response)
        {
            var user = _userRepository.GetByEmail(userLoginModel.Email);

            if (user == null)
            {
                throw new InvalidCredentialException(
                   "The email or password entered is incorrect. Please try again with a different one.");
            }

            if (!IsValidCredentials(userLoginModel, user))
            {
                throw new InvalidCredentialException(
                   "The email or password entered is incorrect. Please try again with a different one.");
            }

            string token = CreateToken(user);
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(user, refreshToken, response);

            return token;
        }

        private bool IsValidCredentials(UserLoginModel userLoginModel, User user)
        {
            if (!VerifyPasswordHash(userLoginModel.Password, user.PasswordSalt, user.PasswordHash))
            {
                return false;
            }

            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string RefreshToken(HttpRequest request, HttpResponse response)
        {
            string token = GetTokenFromRequest(request);
            var userId = GetUserIdFromToken(token);
            var user = _userRepository.GetById(userId);

            var refreshToken = request.Cookies["refreshToken"];
            ValidateToken(user, refreshToken);

            token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(user, newRefreshToken, response);

            return token;
        }

        private string GetTokenFromRequest(HttpRequest request)
        {
            string authorizationHeader = request.Headers["Authorization"];
            string? token = authorizationHeader?.Split(' ').Last();

            if (token == null)
            {
                throw new ArgumentNullException("Token is missing");
            }

            return token;
        }

        private Guid GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new InvalidCredentialException("Invalid user Id in token.");
            }

            return parsedUserId;
        }

        private void ValidateToken(User user, string refreshToken)
        {
            if (!user.RefreshToken.Equals(refreshToken))
            {
                throw new InvalidCredentialException("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                throw new InvalidCredentialException("Token expired.");
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(User user, RefreshToken newRefreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            _userRepository.SaveChanges();
        }
    }
}