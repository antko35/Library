using Library.Core.Entities;
using Library.Persistence.Configurations;
using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence
{
    public class LibraryDbContext : DbContext
    {
      
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) 
            : base(options)
        {
          
        }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleEntity> UserRole { get; set; }
        //public DbSet<UserBookEntity> UserBook { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new RoleConfigurations());
            modelBuilder.ApplyConfiguration(new UserConfigurations());
           // modelBuilder.ApplyConfiguration(new UserBookConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
