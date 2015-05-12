namespace Endjin.Cancelable.Demo
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Cancelable.Azure.Storage;
    using Endjin.Cancelable.Demo.Configuration;

    #endregion

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ensure you are running the Azure Storage Emulator!");
            Console.ResetColor();

            var cancelable = new Cancelable(new CancellationTokenProvider(new ConnectionStringProvider()), new CancellationTokenObserverFactory(new CancellationTokenProvider(new ConnectionStringProvider())));
            var cancellationToken = "E75FF4F5-755E-4FB9-ABE0-24BD81F4D045";

            cancelable.CreateTokenAsync(cancellationToken);
            cancelable.RunUntilCompleteOrCancelledAsync(DoSomethingLongRunningAsync, cancellationToken).Wait();

            Console.WriteLine("Press Any Key to Exit!");
            Console.ReadKey();
        }

        private static async Task DoSomethingLongRunningAsync(CancellationToken cancellationToken)
        {
            int counter = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Doing something {0}", DateTime.Now.ToString("T"));
                
                await Task.Delay(TimeSpan.FromSeconds(1));
                counter++;

                if (counter == 15)
                {
                    Console.WriteLine("Long Running Process Ran to Completion!");
                    break;
                }
            }

            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Long Running Process was Cancelled!");
            }
        }
    }
}