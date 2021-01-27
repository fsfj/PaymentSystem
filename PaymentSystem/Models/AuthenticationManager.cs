using Microsoft.IdentityModel.Tokens;
using PaymentSystem.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{
    public class AuthenticationManager : IAuthenticationManager
    {
        // * create a user and password *
        private readonly IDictionary<string, string> users = new Dictionary<string, string>
        {
            { "usertest1", "password1" },
            { "usertest2", "password2" }
        };

        private readonly string _key;

        public AuthenticationManager (string key){
            _key = key;
        }

        // this process is to create a token for authentication
        public string Authenticate(string username, string password) 
        {
            if (!users.Any(i => i.Key == username && i.Value == password)) // * check if user exists in the table *
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username) // * set username to the claims *
                }),
                Expires = DateTime.UtcNow.AddHours(1), // * set expiration datee for the token *
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
