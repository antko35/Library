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
    public class RoleConfigurations : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(g => g.Id);

            builder
                .HasMany(u => u.Users)
                .WithMany(r => r.Roles)
                .UsingEntity<UserRoleEntity>(
                    l => l.HasOne<UserEntity>().WithMany().HasForeignKey(r => r.UserId),
                    r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(u => u.RoleId)
                    );

        }
    }
}
