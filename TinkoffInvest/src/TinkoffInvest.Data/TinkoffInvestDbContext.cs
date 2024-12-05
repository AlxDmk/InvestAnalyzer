using Microsoft.EntityFrameworkCore;
using TinkoffInvest.Data.Configurations;
using TinkoffInvest.Data.Entities;

namespace TinkoffInvest.Data;

public class TinkoffInvestDbContext(DbContextOptions<TinkoffInvestDbContext> options) : DbContext(options)
{
    public DbSet<SecurityEntity?> Securities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SecurityEntityConfiguration());
    }
}