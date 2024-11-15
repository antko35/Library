using AutoMapper;
using Library.Application.Contracts.User;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Library.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.User
{
    public class RegistrationUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;
        public RegistrationUseCase(IMapper mapper, IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _mapper = mapper;
            _userRepository = unitOfWork.UserRepository;
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }
        public async Task Execute(RegisterRequestDto registerDto)
        {
            await _validationService.ValidateAsync(registerDto);
            var user = await _userRepository.GetByEmail(registerDto.Email);
            var userByName = await _userRepository.GetByUsername(registerDto.UserName);
            if (user != null && userByName != null)
            {
                throw new Exception("Email and Username already in use");
            }
            if (userByName != null)
            {
                throw new Exception("Username already in use");
            }
            if (user != null)
            {
                throw new Exception("Email already in use");
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
