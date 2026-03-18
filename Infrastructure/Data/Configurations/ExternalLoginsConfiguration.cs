using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ExternalLoginsConfiguration : IEntityTypeConfiguration<ExternalLogins>
    {
        public void Configure(EntityTypeBuilder<ExternalLogins> builder)
        {
            builder.Property(u => u.UserId)
                   .IsRequired();

            builder.Property(u => u.Provider)
                   .IsRequired();

            builder.Property(u => u.ProviderId)
                   .IsRequired();

            builder.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            builder.HasOne(u => u.User)
                   .WithMany(u => u.ExternalLogins)
                   .HasForeignKey(u => u.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.Provider, x.ProviderId })
                .IsUnique();
        }
    }
}
