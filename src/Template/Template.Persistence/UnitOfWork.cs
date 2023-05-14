using Template.Application.Contracts;
using Template.Persistence.Database;

namespace Template.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context context;

        public UnitOfWork(Context context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

    }
}
