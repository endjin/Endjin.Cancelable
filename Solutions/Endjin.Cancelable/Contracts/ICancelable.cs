namespace Endjin.Contracts
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface ICancelable
    {
        Task CreateToken(string cancellationToken);

        Task RunUntilCompleteOrCancelledAsync(Func<CancellationToken, Task> action, string cancellationToken);
    }
}