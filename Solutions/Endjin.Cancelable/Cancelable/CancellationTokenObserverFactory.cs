namespace Endjin.Cancelable
{
    #region Using Directives

    using Endjin.Contracts;
    using Endjin.Core.Repeat.Strategies;

    #endregion

    public class CancellationTokenObserverFactory : ICancellationTokenObserverFactory
    {
        private readonly ICancellationTokenProvider cancellationTokenProvider;

        public CancellationTokenObserverFactory(ICancellationTokenProvider cancellationTokenProvider)
        {
            this.cancellationTokenProvider = cancellationTokenProvider;
        }

        public ICancellationTokenObserver Create(string token, IPeriodicityStrategy periodicityStrategy)
        {
            var cancellationTokenObserver = new CancellationTokenObserver(this.cancellationTokenProvider);

            cancellationTokenObserver.StartMonitoring(token, periodicityStrategy);

            return cancellationTokenObserver;
        }
    }
}