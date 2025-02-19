
using Comandas.Api.Data;

namespace Comandas.Api.Repositories
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
