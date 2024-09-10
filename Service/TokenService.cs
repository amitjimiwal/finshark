using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using stockapi.Interface;
using stockapi.Models;

namespace stockapi.Service
{
    public class TokenService : ItokenService
    {
        //appsettings.config file can be accessed through Iconfiguration
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;
        public TokenService(IConfiguration config)
        {
            this.configuration=config;
            this.key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims=new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,user.UserName)
            };

            //create credentials
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var tokenDescriptor=new SecurityTokenDescriptor{
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(7),
                SigningCredentials=creds,
                Issuer=configuration["JWT:Issuer"],
                Audience=configuration["JWT:Audience"]
            };

            var tokenHandler=new JwtSecurityTokenHandler();

            var token=tokenHandler.CreateToken(tokenDescriptor);

            //returns token that is an object created above into a string
            return tokenHandler.WriteToken(token);
            
        }
    }
}