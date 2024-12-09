using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Comment
{
    public class RequestCreateCommentDto
    {
        public Guid BookId { get; set; }
        public string? Comment { get; set; }
        public int Rate { get; set; }
    }
}
