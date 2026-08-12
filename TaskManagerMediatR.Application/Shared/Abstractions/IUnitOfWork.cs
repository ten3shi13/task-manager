namespace TaskManagerMediatR.Application.Shared.Abstractions
{
    public interface IUnitOfWork
    {
        Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
