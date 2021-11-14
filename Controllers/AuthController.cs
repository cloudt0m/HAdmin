using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using HAdmin.Repositories;
using HAdmin.Security;
using HAdmin.Dtos;

namespace HAdmin.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHasher _hasher;
        private readonly ITokenManager _tokenManager;

        public AuthController(IConfiguration configuration, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IHasher hasher, ITokenManager tokenManager)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _hasher = hasher;
            _tokenManager = tokenManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(AuthDto authDto)
        {
            var user = await _userRepository.GetUserByName(authDto.Username);
            if (user == null)
                return Unauthorized();
            var isPasswordValid = _hasher.Check(user.Password, authDto.Password);
            if (!isPasswordValid)
                return Unauthorized();
            var key = _configuration["Jwt:Key"];
            var serializeToken = _tokenManager.GetToken(user.Username);

            HttpContext context = _httpContextAccessor.HttpContext;
            context.Response.Cookies.Append("USER_ID", user.Id.ToString(), new CookieOptions
            {
                Secure = true,
                HttpOnly = false,
                SameSite = SameSiteMode.None,
            });
            context.Response.Cookies.Append("APP-AUTH-COOKIE", serializeToken, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
            });

            return Ok($"User logged in as {user.Username}");
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            // TODO: SameSiteMode.None / Secure = true for dev purpose
            Response.Cookies.Delete("APP-AUTH-COOKIE", new CookieOptions() { SameSite = SameSiteMode.None, Secure = true, });
            Response.Cookies.Delete("USER_ID", new CookieOptions() { SameSite = SameSiteMode.None, Secure = true, });
            return NoContent();
        }

        [HttpGet]
        [Route("is-authenticated")]
        public ActionResult IsAuthenticated()
        {
            bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return Forbid();
            return Ok();
        }
    }
}