namespace Endjin.Contracts
{
    using System.Threading.Tasks;

    public interface ICancellationTokenProvider
    {
        Task CreateAsync(string cancellationToken);

        Task DeleteAsync(string cancellationToken);

        Task<bool> ExistsAsync(string cancellationToken);
    }
}