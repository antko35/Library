using Library.Core.Contracts.User;

namespace Library.Application.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto> Login(LoginRequestUserDto loginDto);
        Task Registration(LoginRequestUserDto registerDto);
    }
}