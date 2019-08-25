using System;
using Adapters;
using Business.Domain;
using Business.Ports;
using Business.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace SaleListenerConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine("Consumer - Hello World!");

            var serviceProvider = new ServiceCollection()
                .AddTransient<ISaleDataProcessor, SaleCsvProcessor>()
                .AddTransient<ISalesContextLoader, SalesContextLoader>()
                .AddTransient<ISalesStatisticsService, SalesStatisticsService>()
                .AddTransient<IFileHelperEngine, SalesFileHelperEngine>()
                .AddTransient<IKafkaConsumer, KafkaConsumer>()
                .BuildServiceProvider();

            var kafkaConsumer = serviceProvider.GetService<IKafkaConsumer>();
            kafkaConsumer.Consume();

        }
    }
}