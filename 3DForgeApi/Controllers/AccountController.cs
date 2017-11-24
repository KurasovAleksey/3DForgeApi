using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using _3DForgeApi.DAL.Model;
using _3DForgeApi.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3DForgeApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IPersonRepository _personRepository;

        private readonly PasswordService _passwordService;

        public AccountController(IPersonRepository personRepository, PasswordService passwordService)
        {
            _personRepository = personRepository;
            _passwordService = passwordService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task Register()
        {
            var username = Request.Form["username"];
            var password = Request.Form["password"];
            var role = Request.Form["role"];

            Person person = (await _personRepository.GetAllPersons())
                .FirstOrDefault(x => x.Login == username);

            if(person != null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync($"User with the login {username} already exists");
                return;
            }

            await _personRepository.AddPerson(new Person()
            {
                Login = username,
                PasswordHash = _passwordService.GetPasswordHash(username + password),
                Role = (Role)byte.Parse(role)
            });
        }

        [HttpPost("token")]
        public async Task Token()
        {
            await InitAdminAsync();
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }
            
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            Person person = (await _personRepository.GetAllPersons())
                .FirstOrDefault(x => x.Login == username 
                                  && x.PasswordHash == _passwordService.GetPasswordHash(username + password));
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        private async Task InitAdminAsync()
        {
            string adminLogin = "admin@gmail.com";
            string adminPassword = "12345678";
            Person admin = (await _personRepository.GetAllPersons()).FirstOrDefault(p => p.Login == adminLogin);
            if(admin == null)
            {
                await _personRepository.AddPerson(new Person()
                {
                    Login = adminLogin,
                    PasswordHash = _passwordService.GetPasswordHash(adminLogin + adminPassword),
                    Role = Role.Admin
                });
            }
        }
    }
}
