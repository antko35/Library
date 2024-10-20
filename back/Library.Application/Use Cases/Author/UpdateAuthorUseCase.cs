using AutoMapper;
using Library.Application.Contracts.Author;
using Library.Core.Abstractions;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Author
{
    public class UpdateAuthorUseCase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;
        public UpdateAuthorUseCase(IUnitOfWork unitOfWork,IMapper mapper, IValidationService validationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }
        public async Task<ResponseAuthorDto> Execete(RequestUpdateAuthorDto requestUpdateAuthorDto)
        {
            await _validationService.ValidateAsync(requestUpdateAuthorDto);
            var authorEntity = await _unitOfWork.AuthorRepository.GetByID(requestUpdateAuthorDto.Id);
            if (authorEntity == null)
            {
                throw new KeyNotFoundException("Author doesn`t exist");
            }

            _mapper.Map(requestUpdateAuthorDto, authorEntity);
            await _unitOfWork.Save();
            var authorForUpdate = _mapper.Map<AuthorEntity>(requestUpdateAuthorDto);
            _unitOfWork.AuthorRepository.Update(authorForUpdate);
            await _unitOfWork.Save();

            var updatedAuthor = await _unitOfWork.AuthorRepository.GetByID(authorEntity.Id);
            var authorResponse = _mapper.Map<ResponseAuthorDto>(updatedAuthor);

            return authorResponse;
        }
    }
}
