using System;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;
using TaskFlow.Infrastructure.Persistence;

namespace TaskFlow.Infrastructure.Repositories;

public class TaskItemRepository : ITaskItemRepository
{
    private readonly TaskDbContext _context;

    public TaskItemRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        => await _context.TaskItems.AddAsync(taskItem, cancellationToken);

    public async Task DeleteAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        => _context.TaskItems.Remove(taskItem);

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TaskItems.ToListAsync(cancellationToken);

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.TaskItems.FindAsync(new object[] { id }, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        => _context.TaskItems.Update(taskItem);
}
