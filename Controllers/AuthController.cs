﻿using HomeBanking.DTOs;
using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService= authService;
        }

        [HttpPost("login")] //esto se ejecutara cuando le llegue una peticion desde "api/auth/login"
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO) //FromBody indica que el valor del parametro para client es tommado de la petición HTTP
        {
            try
            {
                if (!_authService.VerifyData(loginDTO)) {

                }
                Client user = _authService.FindUserByEmail(loginDTO.Email);
                

                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email)
                };
                if (user.Email.ToLower().Equals("ianb.782@live.com"))
                {
                    claims.Add(new Claim("Admin", user.Email));
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return Ok();
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Result()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

    }
}
