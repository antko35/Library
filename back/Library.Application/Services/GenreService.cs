using AutoMapper;
using Library.Core.Abstractions.IRepository;
using Library.Core.Abstractions.IService;
using Library.Core.Contracts.Genre;
using Library.Persistence.Entities;
using Library.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genreRepository = unitOfWork.GenreRepository;
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
        }
        public async Task<List<ResponseGenreDto>> GetAll()
        {
            var genresEntity = await _genreRepository.Get();
            var responseGenre = genresEntity
                .Select(genre => _mapper.Map<ResponseGenreDto>(genre))
                .ToList(); ;

            return responseGenre;
        }
        public async Task<ResponseGenreDto> Create(RequestGenreDto requestGenreDto)
        {
            var genre = await _genreRepository.IsExistByName(requestGenreDto.Genre);
            if (genre != null)
            {
                throw new Exception("Genre already exist");
            }

            var genreEntity = _mapper.Map<GenreEntity>(requestGenreDto);
            await _genreRepository.Insert(genreEntity);
            await _unitOfWork.Save();

            var createdGenre = await _genreRepository.GetByID(genreEntity.Id);
            var genreResponse = _mapper.Map<ResponseGenreDto>(createdGenre);

            return genreResponse;
        }

        public async Task Delete(Guid Id)
        {
            var genre = await _genreRepository.GetByID(Id);
            if (genre == null)
            {
                throw new Exception("Genre doesnt exist");
            }
            await _genreRepository.Delete(Id); 
            await _unitOfWork.Save();
        }
    }
}
