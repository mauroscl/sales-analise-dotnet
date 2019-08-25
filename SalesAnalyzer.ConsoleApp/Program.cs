using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using SalesAnalyzer.Adapters.Primary;
using SalesAnalyzer.Adapters.Secondary;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports.Driven;
using SalesAnalyzer.Application.Ports.Driver;
using SalesAnalyzer.Application.UseCases;

namespace SalesAnalyzer.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Running Sales Analyzer...");

            var serviceProvider = new ServiceCollection()
                .AddTransient<ISaleDataProcessor, SaleCsvProcessor>()
                .AddTransient<ISalesContextLoader, SalesContextLoader>()
                .AddTransient<ISalesStatisticsService, SalesStatisticsService>()
                .AddTransient<IFileHelperEngine, SalesFileHelperEngine>()
                .AddTransient<ISalesAnalyzerPrimaryAdapter, KafkaSalesAnalyzerAdapter>()
                .BuildServiceProvider();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };


            var kafkaConsumer = serviceProvider.GetService<ISalesAnalyzerPrimaryAdapter>();
            kafkaConsumer.ConfigureConsumer(cts.Token);

        }
    }
}