using Comandas.Data.Repositories.Interfaces;

namespace Comandas.Data.Repositories.Implementation
{
    public class Repository : IRepository
    {
        private readonly ComandaDbContext _context;
        public Repository(ComandaDbContext context)
        {
            _context = context;
        }
        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
