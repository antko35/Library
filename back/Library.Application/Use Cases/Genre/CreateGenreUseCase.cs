using AutoMapper;
using Library.Application.DTOs.Genre;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using Library.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Genre
{
    public class CreateGenreUseCase : ICreateGenreUseCase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateGenreUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genreRepository = unitOfWork.GenreRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseGenreDto> Execute(RequestGenreDto requestGenreDto)
        {
            var genre = await _genreRepository.IsExistByName(requestGenreDto.Genre);
            if (genre != null)
            {
                throw new Exception("Genre already exists");
            }

            var genreEntity = _mapper.Map<GenreEntity>(requestGenreDto);
            await _genreRepository.Insert(genreEntity);
            await _unitOfWork.Save();

            var createdGenre = await _genreRepository.GetByID(genreEntity.Id);
            var genreResponse = _mapper.Map<ResponseGenreDto>(createdGenre);

            return genreResponse;
        }
    }
}
