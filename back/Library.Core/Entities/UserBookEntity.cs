using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class UserBookEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid BookId {  get; set; }
        public BookEntity Book { get; set; }
    }
}
