namespace Endjin.Cancelable
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Contracts;
    using Endjin.Core.Composition;

    #endregion

    public class Cancelable : ICancelable
    {
        public async Task CreateTokenAsync(string cancellationToken)
        {
            var cancellationTokenProvider = ApplicationServiceLocator.Container.Resolve<ICancellationTokenProvider>();
            
            await cancellationTokenProvider.CreateAsync(cancellationToken);
        }

        public async Task RunUntilCompleteOrCancelledAsync(Func<CancellationToken, Task> action, string cancellationToken)
        {
            var cancellationTokenObserverFactory = ApplicationServiceLocator.Container.Resolve<ICancellationTokenObserverFactory>();

            using (var cancellationTokenObserver = cancellationTokenObserverFactory.Create(cancellationToken))
            {
                await action(cancellationTokenObserver.CancellationTokenSource.Token);
            }
        }
    }
}