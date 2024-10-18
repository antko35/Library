using AutoMapper;
using Library.Application.DTOs.Genre;
using Library.Core.Abstractions.IRepository;
using Library.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Genre
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _genreRepository = unitOfWork.GenreRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ResponseGenreDto>> Execute()
        {
            var genresEntity = await _genreRepository.Get();
            var responseGenre = genresEntity
                .Select(genre => _mapper.Map<ResponseGenreDto>(genre))
                .ToList(); ;

            return responseGenre;
        }
    }
}
