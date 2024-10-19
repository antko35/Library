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
        public RegistrationUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _userRepository = unitOfWork.UserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(RegisterRequestDto registerDto)
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
