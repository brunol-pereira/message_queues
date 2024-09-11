using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsumidorConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<ConsumerService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
