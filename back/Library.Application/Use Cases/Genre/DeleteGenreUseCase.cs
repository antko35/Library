using AutoMapper;
using Library.Core.Abstractions;
using Library.Core.Abstractions.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Genre
{
    public class DeleteGenreUseCase : IDeleteGenreUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteGenreUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(Guid Id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByID(Id);
            if (genre == null)
            {
                throw new Exception("Genre doesnt exist");
            }
            await _unitOfWork.GenreRepository.Delete(Id);
            await _unitOfWork.Save();
        }
    }
}
