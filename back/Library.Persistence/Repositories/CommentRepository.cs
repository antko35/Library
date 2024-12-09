using Library.Core.Abstractions.IRepository;
using Library.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<CommentEntity>, ICommentRepository
    {
        public CommentRepository(LibraryDbContext context) : base(context) { }

        public async Task<List<CommentEntity>> GetByBook(Guid bookId,int page, int pageSize)
        {
            var comments = await context.Comments
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.Book.Id == bookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            return comments;
        }
        public int GetCount(Guid bookId)
        {
            var count = context
                .Comments
                .AsNoTracking()
                //.Include(x => x.Book)
                .Where(x => x.Book.Id == bookId)
                .Count();
            return count;
        }
    }
}
