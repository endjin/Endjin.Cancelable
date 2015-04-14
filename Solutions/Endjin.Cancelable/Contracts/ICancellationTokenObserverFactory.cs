namespace Endjin.Contracts
{
    using Endjin.Core.Repeat.Strategies;

    public interface ICancellationTokenObserverFactory
    {
        ICancellationTokenObserver Create(string token, IPeriodicityStrategy periodicityStrategy);
    }
}