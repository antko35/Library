﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class CommentEntity : Entity
    {
        public int Rate { get; set; }     
        public string? Comment { get; set; }
        public DateTime? WritingDate { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public BookEntity? Book { get; set; }
        public UserEntity? User { get; set; }
    }
}
