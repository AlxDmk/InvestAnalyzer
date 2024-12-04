using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinkoffInvest.Data.Entities;

namespace TinkoffInvest.Data.Configurations;

public class SecurityEntityConfiguration : IEntityTypeConfiguration<SecurityEntity>
{
    public void Configure(EntityTypeBuilder<SecurityEntity> builder)
    {
        builder.HasKey(s => s.Uid);
    }
}