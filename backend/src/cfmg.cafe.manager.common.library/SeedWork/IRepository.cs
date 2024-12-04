namespace Cfmg.Cafe.Manager.Common.Library.SeedWork
{
    public interface IRepository
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
