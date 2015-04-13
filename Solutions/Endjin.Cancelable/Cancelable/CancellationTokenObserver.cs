namespace Endjin.Cancelable
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Contracts;

    #endregion

    public class CancellationTokenObserver : ICancellationTokenObserver
    {
        private readonly ICancellationTokenProvider cancellationTokenProvider;
        private CancellationTokenSource cancellationTokenSource;
        private Task<Task> scanForTokenTask;

        public CancellationTokenObserver(ICancellationTokenProvider cancellationTokenProvider)
        {
            this.cancellationTokenProvider = cancellationTokenProvider;
        }

        public void Dispose()
        {
            if (this.IsMonitoring)
            {
                this.StopMonitoring();
            }
        }

        public CancellationTokenSource CancellationTokenSource 
        {
            get { return this.cancellationTokenSource ?? (this.cancellationTokenSource = new CancellationTokenSource()); }
        }

        public bool IsMonitoring { get; private set; }

        public void StartMonitoring(string cancellationToken)
        {
            this.scanForTokenTask = Task.Factory.StartNew(async () =>
            {
                while (!this.CancellationTokenSource.IsCancellationRequested)
                {
                    this.CancellationTokenSource.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));

                    if (await this.cancellationTokenProvider.ExistsAsync(cancellationToken))
                    {
                        this.CancellationTokenSource.Cancel();

                        await this.cancellationTokenProvider.DeleteAsync(cancellationToken);
                    }
                }
            },
            this.CancellationTokenSource.Token);

            this.scanForTokenTask.ContinueWith(t => { }, TaskScheduler.Current);
            this.IsMonitoring = true;
        }

        public  void StopMonitoring()
        {
            if (!this.CancellationTokenSource.IsCancellationRequested)
            {
                this.CancellationTokenSource.Cancel();
            }

            this.IsMonitoring = false;
        }
    }
}