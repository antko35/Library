﻿using AutoMapper;
using Library.Application.Contracts.User;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IInfrastructure;
using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.User
{
    public class LoginUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IJWTService _jWTService;
        public LoginUseCase(IMapper mapper, IUnitOfWork unitOfWork, IJWTService jWTService)
        {
            _mapper = mapper;
            _userRepository = unitOfWork.UserRepository;
            _jWTService = jWTService;
        }
        public async Task<LoginResponseDto> Execute(LoginRequestUserDto loginDto)
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
    }
}