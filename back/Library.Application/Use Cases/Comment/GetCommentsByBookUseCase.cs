using AutoMapper;
using Library.Application.Contracts.Comment;
using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Comment
{
    public class GetCommentsByBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCommentsByBookUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;   
        }
        public async Task<List<ResponseCommentDto>> Execute(Guid bookId, int page, int pageSize)
        {
            var comments = await _unitOfWork.CommentRepository.GetByBook(bookId, page, pageSize);
            var response = _mapper.Map<List<ResponseCommentDto>>(comments);
            return response;
        }
    }
}
