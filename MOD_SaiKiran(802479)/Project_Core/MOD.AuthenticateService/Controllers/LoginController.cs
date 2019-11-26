using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MOD.AuthenticateService.Models;
using MOD.AuthenticateService.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MOD.AuthenticateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRep _repository;

        public LoginController(ILoginRep rep)
        {
            _repository = rep;
        }
        // GET: api/Login
        [HttpGet]
        [Route("Validate/User/{email}/{pwd}")]
        public Token Get(string email, string pwd)
        {
            var obj = _repository.UserLogin(email, pwd);
            if (obj == null)
            {
                return new Token() { message = "Invalid User", token = ""};
            }
            else
            {
                return new Token() { message = "User", token = GetToken(), Name =obj.UserName,userID=obj.UID, Block = (bool)obj.UserBlock };
            }
            

        }

        [HttpGet]
        [Route("Validate/Mentor/{email}/{pwd}")]
        public Token Get1(string email, string pwd)
        {
            var obj1 = _repository.MentorLogin(email, pwd);
            if (obj1 == null)
            {
                return new Token() { message = "Invalid User", token = "" };
            }
            else
            {
                return new Token() { message = "User", token = GetToken(),Name =obj1.MentorName,mentorID=obj1.MentorID,Block=(bool)obj1.MentorBlock};
            }
        }

        [HttpGet]
        [Route("Validate/Admin/{email}/{pwd}")]
        public Token Get2(string email, string pwd)
        {
            if (email.Equals("ganesh@gmail.com")&& pwd.Equals("sai"))
            {
                return new Token() { message = "Admin", token = GetToken(),Name="Ganesh",ID=1};
            }
            else
            {
                return new Token() { message = "Invalid Admin", token = "" };
            }
        }

        public string GetToken()
        {
            //return "";
            var _config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials
        (securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer,
        audience: audience,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}
