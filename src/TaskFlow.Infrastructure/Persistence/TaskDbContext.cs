using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Infrastructure.Persistence;

public class TaskDbContext : DbContext
{
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskDbContext).Assembly);
        modelBuilder.Entity<TaskItem>(builder =>
        {
            builder.OwnsOne(t => t.Title, title =>
                {
                    title.Property(t => t.Value)
                    .HasColumnName("Title")
                    .HasMaxLength(100)
                    .IsRequired(); 
                });
        });
    }
}
