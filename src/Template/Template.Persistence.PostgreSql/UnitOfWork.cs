using Template.Application.Contracts;
using Template.Persistence.PosgreSql.Database;

namespace Template.Persistence.PosgreSql
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
