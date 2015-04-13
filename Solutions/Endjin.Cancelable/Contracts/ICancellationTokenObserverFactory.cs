namespace Endjin.Contracts
{
    public interface ICancellationTokenObserverFactory
    {
        ICancellationTokenObserver Create(string token);
    }
}