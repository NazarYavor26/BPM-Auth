using BPM_Auth.BLL.Models;
using BPM_Auth.BLL.Models.User;
using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Entities;
using BPM_Auth.DAL.Enums;
using BPM_Auth.DAL.Repositories;
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
        private UserDataModel _userDataModel;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITeamMembershipRepository _teamMembershipRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly AppDbContext _db;

        public AuthService(
            IConfiguration configuration,
            IUserRepository userRepository,
            ITeamMembershipRepository teamMembershipRepository, 
            ITeamRepository teamRepository,
            AppDbContext db)
        {
            _userDataModel = new UserDataModel();
            _configuration = configuration;
            _userRepository = userRepository;
            _teamMembershipRepository = teamMembershipRepository;
            _teamRepository = teamRepository;
            _db = db;
        }

        public string RegisterAdmin(AdminRegisterModel adminRegisterModel)
        {
            CreatePasswordHash(adminRegisterModel.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = adminRegisterModel.FirstName,
                LastName = adminRegisterModel.LastName,
                Position = adminRegisterModel.Position,
                Email = adminRegisterModel.Email,
                Role = Role.Admin,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            _userRepository.Add(user);

            return user.UserId.ToString();
        }

        public string RegisterMember(UserRegisterModel userRegisterModel)
        {
            CreatePasswordHash(userRegisterModel.Password, out byte[] passwordSalt, out byte[] passwordHash);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FirstName = userRegisterModel.FirstName,
                LastName = userRegisterModel.LastName,
                Position = userRegisterModel.Position,
                Email = userRegisterModel.Email,
                Role = userRegisterModel.Role,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            var team = _teamRepository.GetById(userRegisterModel.TeamId)
                ?? throw new Exception($"Team with Id {userRegisterModel.TeamId} not found");

            var supervisor = _userRepository.GetById(userRegisterModel.SupervisorId)
                ?? throw new Exception($"Supervisor with Id {userRegisterModel.SupervisorId} not found");

            var teamMembership = ConnectUserWithTeam(user, team);
            teamMembership.SupervisorId = supervisor.UserId;
            teamMembership.Supervisor = supervisor;

            user.TeamMemberships.Add(teamMembership);
            team.TeamMemberships.Add(teamMembership);
            _teamMembershipRepository.Add(teamMembership);

            return user.UserId.ToString();
        }

        public string RegisterTeam(TeamRegisterModel teamRegisterModel)
        {
            var team = new Team 
            { 
                TeamId = Guid.NewGuid(),
                TeamName = teamRegisterModel.TeamName
            };

            var user = _userRepository.GetById(teamRegisterModel.AdminId) 
                ?? throw new Exception($"User with Id {teamRegisterModel.AdminId} not found");

            var teamMembership = ConnectUserWithTeam(user, team);
            user.TeamMemberships.Add(teamMembership);
            team.TeamMemberships.Add(teamMembership);
            _teamMembershipRepository.Add(teamMembership);

            return team.TeamId.ToString();
        }

        private TeamMembership ConnectUserWithTeam(User user, Team team, User? Supervisor = null)
        {
            return new TeamMembership
            {
                TeamMembershipId = Guid.NewGuid(),
                UserId = user.UserId,
                User = user,
                TeamId = team.TeamId,
                Team = team,
                SupervisorId = Supervisor?.UserId,
                Supervisor = Supervisor
            };
        }

        public string Login(UserLoginModel userLoginModel, HttpResponse response)
        {
            if (!IsValidCredentials(userLoginModel))
            {
                throw new InvalidCredentialException(
                    "The email or password entered is incorrect. Please try again with a different one.");
            }

            string token = CreateToken(_userDataModel);
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, response);

            return token;
        }

        public string RefreshToken(HttpRequest request, HttpResponse response)
        {
            var refreshToken = request.Cookies["refreshToken"];

            if (!_userDataModel.RefreshToken.Equals(refreshToken))
            {
                throw new InvalidCredentialException("Invalid Refresh Token.");
            }
            else if (_userDataModel.TokenExpires < DateTime.Now)
            {
                throw new InvalidCredentialException("Token expired.");
            }

            string token = CreateToken(_userDataModel);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, response);

            return token;
        }

        private bool IsValidCredentials(UserLoginModel userLoginModel)
        {
            if (userLoginModel.Email != _userDataModel.Email)
            {
                return false;
            }

            if (!VerifyPasswordHash(userLoginModel.Password, _userDataModel.PasswordSalt, _userDataModel.PasswordHash))
            {
                return false;
            }

            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(UserDataModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

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

        private void SetRefreshToken(RefreshToken newRefreshToken, HttpResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };

            response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            _userDataModel.RefreshToken = newRefreshToken.Token;
            _userDataModel.TokenCreated = newRefreshToken.Created;
            _userDataModel.TokenExpires = newRefreshToken.Expires;
        }
    }
}