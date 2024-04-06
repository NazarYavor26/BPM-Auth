using BPM.BLL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

        public string Login(UserLoginModel userLoginModel)
        {
            if (userLoginModel.Email != _userDataModel.Email)
            {
                return "Email or password is incorrect";
            }

            if (!VerifyPasswordHash(userLoginModel.Password, _userDataModel.PasswordSalt, _userDataModel.PasswordHash))
            {
                return "Email or password is incorrect";
            }

            string token = CreateToken(_userDataModel);

            return token;
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
    }
}