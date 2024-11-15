using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts.User;
using Library.Application.Use_Cases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly IValidator<LoginRequestUserDto> _loginValidator;
        private readonly GetInfoUseCase _infoUseCase;
        private readonly LoginUseCase _loginUseCase;
        private readonly RegistrationUseCase _registrationUseCase;
        public UserController(
            IValidator<RegisterRequestDto> regValidator, 
            IValidator<LoginRequestUserDto> loginValidator,
            GetInfoUseCase infoUseCase,
            LoginUseCase loginUseCase,
            RegistrationUseCase registrationUseCase)
        {
            _registerValidator = regValidator;
            _loginValidator = loginValidator;
            _infoUseCase = infoUseCase;
            _loginUseCase = loginUseCase;
            _registrationUseCase = registrationUseCase;
        }

        [HttpGet("info")]
        public async Task<ActionResult<ResponseUserInfoDto>> GetInfo()
        {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);
                var response = await _infoUseCase.Execute(UserId);
                response.Role = User.Claims.First(x => x.Type == "Role").Value;
                return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestUserDto loginDto)
        {
                var token = await _loginUseCase.Execute(loginDto);
                HttpContext.Response.Cookies.Append("jwt_cookie", token.access_token);

                return Ok(token);
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("jwt_cookie");

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Registration(RegisterRequestDto regDto)
        {
                await _registrationUseCase.Execute(regDto);
                return Ok();
        }
    }
}
