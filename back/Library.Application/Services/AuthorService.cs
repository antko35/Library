using AutoMapper;
using Library.Core.Contracts.Author;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
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
        private readonly IMapper _mapper;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }
        public async Task<List<ResponseAuthorDto>> GetAll()
        {
            var authors = await _authorRepository.GetAll();

            var authorResponse = authors
                .Select(author => _mapper.Map<ResponseAuthorDto>(author))
                .ToList();

            return authorResponse;
        }
        public async Task<ResponseAuthorDto> GetById(Guid id)
        {
            var author = await _authorRepository.GetById(id);
            if (author == null)
            {
                throw new Exception("Author doesnt exist");
            }

            var authorResponse = _mapper.Map<ResponseAuthorDto>(author);

            return authorResponse;
        }
        public async Task<ResponseAuthorDto> Create(RequestAuthorDto requestAuthorDto)
        {
            bool isExist = await _authorRepository.IsExist(requestAuthorDto.Name, requestAuthorDto.Surname, requestAuthorDto.BirthDate);
            if (isExist)
            {
                throw new Exception("Author already exist");
            }

            var authorEntity = _mapper.Map<AuthorEntity>(requestAuthorDto);
            await _authorRepository.Create(authorEntity);
            var authorResponse = _mapper.Map<ResponseAuthorDto>(authorEntity);
            return authorResponse;
        }

        public async Task<ResponseAuthorDto> Update(RequestUpdateAuthorDto requestUpdateAuthorDto)
        {
            var authorEntity = await _authorRepository.GetById(requestUpdateAuthorDto.Id);
            if (authorEntity == null)
            {
                throw new Exception("Author doesn`t exist");
            }

            var authorForUpdate = _mapper.Map<AuthorEntity>(requestUpdateAuthorDto);
            await _authorRepository.Update(authorForUpdate);

            var updatedAuthor = await _authorRepository.GetById(authorEntity.Id);
            var authorResponse = _mapper.Map<ResponseAuthorDto>(updatedAuthor);

            return authorResponse;
        }

        public async Task Delete(Guid Id)
        {
            var count = await _authorRepository.Delete(Id);

            if (count == 0)
            {
                throw new Exception("Author doesnt exist");
            }
        }
    }
}
