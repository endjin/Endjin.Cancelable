namespace Endjin.Contracts
{
    #region Using Directives

    using System;
    using System.Threading;

    #endregion

    public interface ICancellationTokenObserver : IDisposable
    {
        CancellationTokenSource CancellationTokenSource { get; }

        bool IsMonitoring { get; }

        void StartMonitoring(string cancellationToken);

        void StopMonitoring();
    }
}