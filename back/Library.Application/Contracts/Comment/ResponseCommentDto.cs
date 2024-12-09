using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Comment
{
    public class ResponseCommentDto
    {
        public int Rate { get; set; }
        public DateTime WritingDate { get; set; }
        public string? Comment { get; set; }
        public string? Author { get; set; }
    }
}
