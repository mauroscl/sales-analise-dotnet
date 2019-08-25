using System;
using FileHelpers;
using Microsoft.Extensions.DependencyInjection;
using SalesAnalyzer.Adapters;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports;
using SalesAnalyzer.Application.UseCases;

namespace SalesAnalyzer.ConsoleApp
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