using Library.Core.Contracts.User;

namespace Library.Core.Abstractions.IService
{
    public interface IUserService
    {
        Task<ResponseUserInfoDto> GetInfo(Guid userId);
        Task<LoginResponseDto> Login(LoginRequestUserDto loginDto);
        Task Registration(RegisterRequestDto registerDto);
    }
}