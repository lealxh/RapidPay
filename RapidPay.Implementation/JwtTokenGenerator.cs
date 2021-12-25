using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RapidPay.Domain;
using RapidPay.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RapidPay.Services
{
    /// <summary>
    /// Service for JWT token generation
    /// </summary>
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string key;

        public JwtTokenGenerator(IOptions<AppSettings> config)
        {

            this.key = config.Value.Key;
        }
        public string CreateToken(string username)
        {
            // Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            //Create Private Key
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //Create descriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //Return Token from method
            return tokenHandler.WriteToken(token);
        }
    }
}
