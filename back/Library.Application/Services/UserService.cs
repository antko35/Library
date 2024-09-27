using AutoMapper;
using FluentValidation;
using Library.Application.Authorization;
using Library.Core.Contracts.User;
using Library.Core.Entities;
using Library.Core.Enums;
using Library.Persistence.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<LoginResponseDto> Login(LoginRequestUserDto loginDto)
        {
            var userEntity = _mapper.Map<UserEntity>(loginDto);
            var user = await _userRepository.Get(userEntity);
            if (user == null)
            {
                throw new Exception("invalid data");
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Roles.First().RoleName)
            };

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            LoginResponseDto response = new LoginResponseDto();
            response.access_token = encodedJwt;
            response.username = user.Email;

            return response;

            /*Person? person = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
            // если пользователь не найден, отправляем статусный код 401
            if (person is null) return Results.Unauthorized();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);*/
        }

        public async Task Registration(LoginRequestUserDto registerDto)
        {
            var user = await _userRepository.GetByEmail(registerDto.Email);
            if (user != null)
            {
                throw new Exception("Invalid email");
            }
            var userEntity = _mapper.Map<UserEntity>(registerDto);

            var userRole = await _userRepository.GetRole(Role.Admin);
            if (userRole == null) { throw new Exception("Problems with role"); }

            userEntity.Roles = new[] { userRole };

            await _userRepository.Register(userEntity);
        }
    }
}
