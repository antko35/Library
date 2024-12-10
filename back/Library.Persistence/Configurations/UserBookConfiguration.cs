using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Configurations
{
    public class UserBookConfiguration : IEntityTypeConfiguration<UserBookEntity>
    {
        public void Configure(EntityTypeBuilder<UserBookEntity> builder)
        {
            builder.HasKey(x => new {x.UserId, x.BookId });

           /* builder
                .HasOne(ub => ub.User)
                .WithMany(u => u.Books)
                .HasForeignKey(ub => ub.UserId);

            builder
                .HasOne(ub => ub.Book)
                .WithMany(b => b.Users)
                .HasForeignKey(ub => ub.BookId);*/
        }
    }
}
