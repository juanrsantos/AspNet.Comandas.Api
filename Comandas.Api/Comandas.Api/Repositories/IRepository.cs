namespace Comandas.Api.Repositories
{
    public interface IRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
