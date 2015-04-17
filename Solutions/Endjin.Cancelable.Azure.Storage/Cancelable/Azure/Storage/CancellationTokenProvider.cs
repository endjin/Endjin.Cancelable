namespace Endjin.Cancelable.Azure.Storage
{
    #region Using Directives

    using System.IO;
    using System.Threading.Tasks;

    using Endjin.Contracts;
    using Endjin.Core.Retry;

    using Microsoft.Azure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    #endregion

    public class CancellationTokenProvider : ICancellationTokenProvider
    {
        private readonly IConnectionStringProvider connectionStringProvider;
        
        private CloudBlobClient client;
        private CloudBlobContainer container;
        private bool initialised;
        private CloudStorageAccount storageAccount;

        public CancellationTokenProvider(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public async Task CreateAsync(string cancellationToken)
        {
            await this.InitialiseAsync();

            var blob = this.container.GetBlockBlobReference(cancellationToken);

            await Retriable.RetryAsync(async () => 
            {
                using (var ms = new MemoryStream())
                {
                    await blob.UploadFromStreamAsync(ms);
                }
            });
        }

        public async Task DeleteAsync(string cancellationToken)
        {
            await this.InitialiseAsync();

            var blob = this.container.GetBlockBlobReference(cancellationToken);

            await Retriable.RetryAsync(() => blob.DeleteAsync());
        }

        public async Task<bool> ExistsAsync(string cancellationToken)
        {
            await this.InitialiseAsync();

            var blob = this.container.GetBlockBlobReference(cancellationToken);

            return await Retriable.RetryAsync(blob.ExistsAsync);
        }

        private async Task InitialiseAsync()
        {
            if (!this.initialised)
            {
                this.storageAccount = CloudStorageAccount.Parse(Configuration.GetSettingFor(this.connectionStringProvider.ConnectionStringKey));
                this.client = this.storageAccount.CreateCloudBlobClient();
                this.container = this.client.GetContainerReference("endjin-cancelable-tokens");

                if (await Retriable.RetryAsync(() => this.container.CreateIfNotExistsAsync()))
                {
                    var containerPermissions = new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off };

                    await Retriable.RetryAsync(() => this.container.SetPermissionsAsync(containerPermissions));
                }

                this.initialised = true;
            }
        }
    }
}