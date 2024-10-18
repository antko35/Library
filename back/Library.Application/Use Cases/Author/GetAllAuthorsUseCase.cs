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
    public class GetAllAuthorsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllAuthorsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<ResponseAuthorDto>> Execute()
        {
            var authors = await _unitOfWork.AuthorRepository.Get();

            var authorResponse = authors
                .Select(author => _mapper.Map<ResponseAuthorDto>(author))
                .ToList();

            return authorResponse;
        }
    }
}
