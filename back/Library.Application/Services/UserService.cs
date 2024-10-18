using AutoMapper;
using FluentValidation;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions.IService;
using Library.Core.Contracts.User;
using Library.Core.Entities;
using Library.Core.Enums;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJWTService _jWTService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {
            _userRepository = unitOfWork.UserRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jWTService = jWTService;
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

            var encodedJwt = _jWTService.Gerenate(user);

            LoginResponseDto response = new LoginResponseDto
            {
                access_token = encodedJwt,
                username = user.UserName
            };

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

            await _unitOfWork.UserRepository.Insert(userEntity);
            await _unitOfWork.Save();
        }
    }
}
