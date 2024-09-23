using AutoMapper;
using Library.Core.Contracts.Genre;
using Library.Persistence.Entities;
using Library.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        public async Task<List<ResponseGenreDto>> GetAll()
        {
            var genresEntity = await _genreRepository.GetAll();
            var responseGenre = genresEntity
                .Select(genre => _mapper.Map<ResponseGenreDto>(genre))
                .ToList(); ;

            return responseGenre;
        }
        public async Task<ResponseGenreDto> Create(RequestGenreDto requestGenreDto)
        {
            var isExist = await _genreRepository.IsExistByName(requestGenreDto.Genre);
            if (isExist)
            {
                throw new Exception("Genre already exist");
            }

            var genreEntity = _mapper.Map<GenreEntity>(requestGenreDto);
            await _genreRepository.Create(genreEntity);
            var genreResponse = _mapper.Map<ResponseGenreDto>(genreEntity);

            return genreResponse;
               
        }
    }
}
