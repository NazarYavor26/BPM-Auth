using BPM.BLL.Models;
using BPM.BLL.Models.User;
using BPM.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BPM.BLL.Services
{
    public class AuthService : IAuthService
    {
        private UserDataModel _userDataModel;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _userDataModel = new UserDataModel();
            _configuration = configuration;
        }

        public string Register(UserRegisterModel userRegisterModel)
        {
            CreatePasswordHash(userRegisterModel.Password, out byte[] passwordSalt, out byte[] passwordHash);

            _userDataModel.FirstName = userRegisterModel.FirstName;
            _userDataModel.LastName = userRegisterModel.LastName;
            _userDataModel.Email = userRegisterModel.Email;
            _userDataModel.PasswordSalt = passwordSalt;
            _userDataModel.PasswordHash = passwordHash;

            return _userDataModel.Email;
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