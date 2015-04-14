namespace Endjin.Cancelable.Specs.Configuration
{
    using Endjin.Contracts;

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionStringKey
        {
            get { return "Storage.CancellationProvider.ConnectionString"; }
        }
    }
}