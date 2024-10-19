using Library.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Use_Cases.Author
{
    public class DeleteAuthorUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAuthorUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Execute(Guid Id)
        {
            var author = await _unitOfWork.AuthorRepository.GetByID(Id);
            if (author == null)
            {
                throw new KeyNotFoundException("author not found");
            }
            await _unitOfWork.AuthorRepository.Delete(Id);
            await _unitOfWork.Save();
        }
    }
}
