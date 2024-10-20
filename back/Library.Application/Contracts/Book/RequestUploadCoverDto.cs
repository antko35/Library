using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Book
{
    public class RequestUploadCoverDto
    {
        public Guid BookId { get; set; }
        public IFormFile? File { get; set; }
    }
}
