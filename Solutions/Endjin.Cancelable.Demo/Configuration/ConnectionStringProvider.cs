namespace Endjin.Cancelable.Demo.Configuration
{
    using Endjin.Contracts;

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get { return "Storage.CancellationProvider.ConnectionString"; }
        }
    }
}