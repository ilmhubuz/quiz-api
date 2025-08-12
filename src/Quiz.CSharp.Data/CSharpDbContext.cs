namespace Quiz.CSharp.Data;

using Microsoft.EntityFrameworkCore;
using Quiz.CSharp.Data.Entities;
using System.Reflection;

public interface ICSharpDbContext
{
    DbSet<Collection> Collections { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<UserAnswer> UserAnswers { get; set; }
    DbSet<UserProgress> UserProgress { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public sealed class CSharpDbContext(DbContextOptions<CSharpDbContext> options) : DbContext(options), ICSharpDbContext
{
    public DbSet<Collection> Collections { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<UserProgress> UserProgress { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimestamps()
    {
        var now = DateTime.UtcNow;
        var entries = ChangeTracker.Entries<IHasTimestamp>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}