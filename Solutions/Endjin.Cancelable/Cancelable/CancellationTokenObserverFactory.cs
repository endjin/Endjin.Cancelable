namespace Endjin.Cancelable
{
    #region Using Directives

    using Endjin.Contracts;

    #endregion

    public class CancellationTokenObserverFactory : ICancellationTokenObserverFactory
    {
        private readonly ICancellationTokenProvider cancellationTokenProvider;

        public CancellationTokenObserverFactory(ICancellationTokenProvider cancellationTokenProvider)
        {
            this.cancellationTokenProvider = cancellationTokenProvider;
        }

        public ICancellationTokenObserver Create(string token)
        {
            var cancellationTokenObserver = new CancellationTokenObserver(this.cancellationTokenProvider);

            cancellationTokenObserver.StartMonitoring(token);

            return cancellationTokenObserver;
        }
    }
}