namespace Comandas.Data.Repositories.Interfaces
{
    public interface IRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
