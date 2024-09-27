using Library.Application.Authorization;
using Library.Application.Services;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /*[HttpGet]
        public async Task<ActionResult> GetInfo()
        {
            return Ok();
        }*/

        [HttpPost("login/")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestUserDto loginDto)
        {
            try
            {
                var token = await _userService.Login(loginDto);
                HttpContext.Response.Cookies.Append("jwt_cookie", token.access_token);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPost("register")]
        public async Task<ActionResult> Registration(LoginRequestUserDto loginDto)
        {
            try
            {
                await _userService.Registration(loginDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
