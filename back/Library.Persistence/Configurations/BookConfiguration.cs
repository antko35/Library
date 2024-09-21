using Library.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.HasKey(b => b.Id);

            builder.
                HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(a => a.GenreId);

            builder
                .HasOne(b => b.Genre)
                .WithMany(g => g.Books)
                .HasForeignKey(g => g.GenreId);
        }
    }
}
