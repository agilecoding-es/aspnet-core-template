namespace Template.Application.Contracts
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
