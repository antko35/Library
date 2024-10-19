using AutoMapper;
using Library.Application.Contracts.Author;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Author
{
    public class GetAuthorByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAuthorByIdUseCase(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseAuthorDto> Execute(Guid id)
        {
            var author = await _unitOfWork.AuthorRepository.GetByID(id);
            if (author == null)
            {
                throw new KeyNotFoundException("Author doesnt exist");
            }

            var authorResponse = _mapper.Map<ResponseAuthorDto>(author);

            return authorResponse;
        }
    }
}
