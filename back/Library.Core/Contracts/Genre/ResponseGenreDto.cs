using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.Genre
{
    public class ResponseGenreDto
    {
        public Guid Id { get; set; }
        public string Genre { get; set; } = string.Empty;
    }
}
