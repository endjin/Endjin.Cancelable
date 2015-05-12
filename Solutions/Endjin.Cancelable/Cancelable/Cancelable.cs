namespace Endjin.Cancelable
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Contracts;
    using Endjin.Core.Repeat.Strategies;

    #endregion

    public class Cancelable : ICancelable
    {
        private readonly ICancellationTokenProvider cancellationTokenProvider;
        private readonly ICancellationTokenObserverFactory cancellationTokenObserverFactory;

        public Cancelable(ICancellationTokenProvider cancellationTokenProvider, ICancellationTokenObserverFactory cancellationTokenObserverFactory)
        {
            this.cancellationTokenProvider = cancellationTokenProvider;
            this.cancellationTokenObserverFactory = cancellationTokenObserverFactory;
        }

        public async Task CreateTokenAsync(string cancellationToken)
        {
            await this.cancellationTokenProvider.CreateAsync(cancellationToken);
        }

        public async Task<CancelableResult> RunUntilCompleteOrCancelledAsync(Func<CancellationToken, Task> action, string cancellationToken, IPeriodicityStrategy periodicityStrategy = null)
        {
            if (periodicityStrategy == null)
            {
                periodicityStrategy = new LinearPeriodicityStrategy(TimeSpan.FromSeconds(5));
            }

            var cancelableResult = CancelableResult.Completed;

            using (var cancellationTokenObserver = this.cancellationTokenObserverFactory.Create(cancellationToken, periodicityStrategy))
            {
                await action(cancellationTokenObserver.CancellationTokenSource.Token);
                if (cancellationTokenObserver.CancellationTokenSource.IsCancellationRequested)
                {
                    cancelableResult = CancelableResult.Cancelled;
                }
            }

            return cancelableResult;
        }

        public async Task DeleteTokenAsync(string cancellationToken)
        {
            await this.cancellationTokenProvider.DeleteAsync(cancellationToken);
        }
    }
}