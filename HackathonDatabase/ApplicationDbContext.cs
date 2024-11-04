using HackathonDatabase.model;
using Microsoft.EntityFrameworkCore;

namespace HackathonDatabase;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<HackathonEntity> HackathonEntities { get; set; }
    public DbSet<EmployeeEntity> EmployeeEntities { get; set; }
    public DbSet<TeamEntity> TeamEntities { get; set; }
    public DbSet<WishlistEntity> WishlistEntities { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<EmployeeEntity>()
            .HasKey(e => new { e.Id, e.EmployeeType });
    }
}