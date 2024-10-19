using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Contracts.User
{
    public class ResponseUserInfoDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<BookEntity> Books { get; set;} = new List<BookEntity>();
    }
}
