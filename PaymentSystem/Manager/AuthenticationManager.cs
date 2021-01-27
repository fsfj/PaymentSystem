using Microsoft.IdentityModel.Tokens;
using PaymentSystem.Interface;
using PaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSystem.Manager
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly DatabaseContext _DatabaseContext;

        public AuthenticationManager(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        // this process is to create a token for authentication
        public string Authenticate(string username, string password) 
        {
            Encryptor encryptor = new Encryptor();
            var eK = encryptor.EncryptionKey();

            var encryptedPassword = encryptor.Encrypt(password, eK); // this is to assume that password is ecrypted

            if (!_DatabaseContext.UserCredentials.Any(i => i.Username == username && i.Password == encryptedPassword)) // * check if user exists in the table *
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("this is the key for my token");
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
