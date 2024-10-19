using AutoMapper;
using Library.Application.Contracts.Author;
using Library.Core.Abstractions;
using Library.Core.Entities;
using System.Data.SqlTypes;

namespace Library.Application.Use_Cases.Author
{
    public class CreateAuthorUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CreateAuthorUseCase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseAuthorDto> Execute(RequestAuthorDto requestAuthorDto)
        {
            bool isExist = await _unitOfWork.AuthorRepository.IsExist(requestAuthorDto.Name, requestAuthorDto.Surname, requestAuthorDto.BirthDate);
            if (isExist)
            {
                throw new InvalidOperationException("Author already exist");
            }

            var authorEntity = _mapper.Map<AuthorEntity>(requestAuthorDto);

            await _unitOfWork.AuthorRepository.Insert(authorEntity);
            await _unitOfWork.Save();

            var authorResponse = _mapper.Map<ResponseAuthorDto>(authorEntity);
            return authorResponse;
        }
    }
}
