using AutoMapper;
using Library.Application.Contracts.User;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.User
{
    public class GetInfoUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetInfoUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = unitOfWork.UserRepository;
            _mapper = mapper;
        }
        public async Task<ResponseUserInfoDto> Execute(Guid userId)
        {
            var info = await _userRepository.GetInfo(userId);
            var response = _mapper.Map<ResponseUserInfoDto>(info);
            return response;
        }
    }
}
