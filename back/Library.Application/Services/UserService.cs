using AutoMapper;
using FluentValidation;
using Library.Application.Authorization;
using Library.Core.Contracts.User;
using Library.Core.Entities;
using Library.Core.Enums;
using Library.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ResponseUserInfoDto> GetInfo(Guid userId)
        {
            var info = await _userRepository.GetInfo(userId);
            var response  = _mapper.Map<ResponseUserInfoDto>(info);
            return response;
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
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(30)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            LoginResponseDto response = new LoginResponseDto();
            response.access_token = encodedJwt;
            response.username = user.UserName;

            return response;
        }

        public async Task Registration(RegisterRequestDto registerDto)
        {
            var user = await _userRepository.GetByEmail(registerDto.Email);
            if (user != null)
            {
                throw new Exception("Invalid email");
            }
            var userEntity = _mapper.Map<UserEntity>(registerDto);

            var userRole = await _userRepository.GetRole(Role.User); //для создания админа изменить здесь
            if (userRole == null) { throw new Exception("Problems with role"); }

            userEntity.Roles = new[] { userRole };

            await _userRepository.Register(userEntity);
        }
    }
}
