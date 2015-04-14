namespace Endjin.Contracts
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Cancelable;
    using Endjin.Core.Repeat.Strategies;

    #endregion

    public interface ICancelable
    {
        Task CreateTokenAsync(string cancellationToken);

        Task<CancelableResult> RunUntilCompleteOrCancelledAsync(Func<CancellationToken, Task> action, string cancellationToken, IPeriodicityStrategy periodicityStrategy = null);

        Task DeleteTokenAsync(string cancellationToken);
    }
}