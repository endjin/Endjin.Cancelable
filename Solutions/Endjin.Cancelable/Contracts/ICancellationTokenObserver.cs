namespace Endjin.Contracts
{
    #region Using Directives

    using System;
    using System.Threading;

    using Endjin.Core.Repeat.Strategies;

    #endregion

    public interface ICancellationTokenObserver : IDisposable
    {
        CancellationTokenSource CancellationTokenSource { get; }

        bool IsMonitoring { get; }

        void StartMonitoring(string cancellationToken, IPeriodicityStrategy periodicityStrategy);

        void StopMonitoring();
    }
}