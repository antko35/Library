using AutoMapper;
using Library.Core.Contracts.Author;
using Library.Core.Contracts.Book;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using Library.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class AuthorService : IAuthorService
    {
        
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authorRepository = unitOfWork.AuthorRepository;
            _mapper = mapper;
        }
        public async Task<List<ResponseAuthorDto>> GetAll()
        {
            var authors = await _authorRepository.Get();

            var authorResponse = authors
                .Select(author => _mapper.Map<ResponseAuthorDto>(author))
                .ToList();

            return authorResponse;
        }
        public async Task<ResponseAuthorDto> GetById(Guid id)
        {
            var author = await _authorRepository.GetByID(id);
            if (author == null)
            {
                throw new Exception("Author doesnt exist");
            }

            var authorResponse = _mapper.Map<ResponseAuthorDto>(author);

            return authorResponse;
        }

        public async Task<List<ResponseBookDto>> GetBooksByAuthor(Guid authorId)
        {
            var authorEntity = await _authorRepository.GetByID(authorId);

            if (authorEntity == null)
            {
                throw new Exception("Author does not exist");
            }

            var books = await _authorRepository.GetBookByAuthor(authorId);

            var booksResponse = books
                .Select(b => _mapper.Map<ResponseBookDto>(b))
                .ToList();

            return booksResponse;
        }

        public async Task<ResponseAuthorDto> Create(RequestAuthorDto requestAuthorDto)
        {
            bool isExist = await _authorRepository.IsExist(requestAuthorDto.Name, requestAuthorDto.Surname, requestAuthorDto.BirthDate);
            if (isExist)
            {
                throw new Exception("Author already exist");
            }

            var authorEntity = _mapper.Map<AuthorEntity>(requestAuthorDto);

            await _unitOfWork.AuthorRepository.Insert(authorEntity);
            await _unitOfWork.Save();

            var authorResponse = _mapper.Map<ResponseAuthorDto>(authorEntity);
            return authorResponse;
        }

        public async Task<ResponseAuthorDto> Update(RequestUpdateAuthorDto requestUpdateAuthorDto)
        {
            var authorEntity = await _authorRepository.GetByID(requestUpdateAuthorDto.Id);
            if (authorEntity == null)
            {
                throw new Exception("Author doesn`t exist");
            }

            _mapper.Map(requestUpdateAuthorDto, authorEntity);
            await _unitOfWork.Save();
            /*var authorForUpdate = _mapper.Map<AuthorEntity>(requestUpdateAuthorDto);
            _authorRepository.Update(authorForUpdate);
            await _unitOfWork.Save();*/

            var updatedAuthor = await _authorRepository.GetByID(authorEntity.Id);
            var authorResponse = _mapper.Map<ResponseAuthorDto>(updatedAuthor);

            return authorResponse;
        }

        public async Task Delete(Guid Id)
        {
            var author = await _authorRepository.GetByID(Id);
            if (author == null)
            {
                throw new Exception("not found");
            }
            await _authorRepository.Delete(Id);
            await _unitOfWork.Save();
        }
    }
}
