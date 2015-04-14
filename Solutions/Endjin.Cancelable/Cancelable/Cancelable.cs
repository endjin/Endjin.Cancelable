namespace Endjin.Cancelable
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Contracts;
    using Endjin.Core.Composition;
    using Endjin.Core.Repeat.Strategies;

    #endregion

    public class Cancelable : ICancelable
    {
        public async Task CreateTokenAsync(string cancellationToken)
        {
            var cancellationTokenProvider = ApplicationServiceLocator.Container.Resolve<ICancellationTokenProvider>();
            
            await cancellationTokenProvider.CreateAsync(cancellationToken);
        }

        public async Task<CancelableResult> RunUntilCompleteOrCancelledAsync(Func<CancellationToken, Task> action, string cancellationToken, IPeriodicityStrategy periodicityStrategy = null)
        {
            if (periodicityStrategy == null)
            {
                periodicityStrategy = new LinearPeriodicityStrategy(TimeSpan.FromSeconds(5));
            }

            var cancellationTokenObserverFactory = ApplicationServiceLocator.Container.Resolve<ICancellationTokenObserverFactory>();

            var cancelableResult = CancelableResult.Completed;

            using (var cancellationTokenObserver = cancellationTokenObserverFactory.Create(cancellationToken, periodicityStrategy))
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
            var cancellationTokenProvider = ApplicationServiceLocator.Container.Resolve<ICancellationTokenProvider>();

            await cancellationTokenProvider.DeleteAsync(cancellationToken);
        }
    }
}