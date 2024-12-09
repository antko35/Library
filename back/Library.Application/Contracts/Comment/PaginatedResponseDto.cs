using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Contracts.Comment
{
    public class PaginatedResponseDto<T>
    {
        public List<T> Items { get; set; } // Список элементов
        public int TotalCount { get; set; } // Общее количество записей
    }

}
