using FluentValidation;
using FluentValidation.Results;
using Library.Application.Contracts.User;
using Library.Application.Use_Cases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
            try
            {
                var userId = User.Claims.First(x => x.Type == "UserId").Value;
                Guid UserId = Guid.Parse(userId);
                var response = await _infoUseCase.Execute(UserId);
                return Ok(response);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestUserDto loginDto)
        {
            ValidationResult result = await _loginValidator.ValidateAsync(loginDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                var token = await _loginUseCase.Execute(loginDto);
                HttpContext.Response.Cookies.Append("jwt_cookie", token.access_token);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
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
            ValidationResult result = await _registerValidator.ValidateAsync(regDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await _registrationUseCase.Execute(regDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }
    }
}
