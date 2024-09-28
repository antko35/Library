﻿using FluentValidation;
using FluentValidation.Results;
using Library.Application.Authorization;
using Library.Application.Services;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.User;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly IValidator<LoginRequestUserDto> _loginValidator;
        public UserController(IUserService userService, IValidator<RegisterRequestDto> regValidator, IValidator<LoginRequestUserDto> loginValidator)
        {
            _userService = userService;
            _registerValidator = regValidator;
            _loginValidator = loginValidator;
        }

       /* [HttpGet]
        public async Task<ActionResult> GetInfo()
        {
            return Ok();
        }*/

        [HttpPost("login/")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestUserDto loginDto)
        {
            ValidationResult result = await _loginValidator.ValidateAsync(loginDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

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
        public async Task<ActionResult> Registration(RegisterRequestDto regDto)
        {
            ValidationResult result = await _registerValidator.ValidateAsync(regDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await _userService.Registration(regDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}