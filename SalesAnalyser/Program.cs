using System;
using System.IO;
using InfraStructure;
using Microsoft.Extensions.DependencyInjection;

namespace SalesAnalyser
{
    internal class Program
    {
        //private const string InputPath = "d:\\data\\in";
        //private const string OutputPath = "d:\\data\\out";

        private const string InputPath = "data\\in";
        private const string OutputPath = "data\\out";
        private static IServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {
            Console.WriteLine("Base Path: " + AppDomain.CurrentDomain.BaseDirectory);

            RegistryServices();

            var fileService = _serviceProvider.GetService<IFileService>();

            fileService.CreateApplicationDirectories(InputPath, OutputPath);

            ProcessExistingFiles(fileService);

            InitializeFileWatcher();
        }

        private static void RegistryServices()
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<ISalesSummaryOutputService, SalesSummaryOutputFileService>()
                .AddTransient<IFileService, FileService>()
                .AddTransient<IKafkaProducer, KafkaProducer>()
                .BuildServiceProvider();
        }


        private static void InitializeFileWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            using (var watcher = new FileSystemWatcher())
            {
                watcher.IncludeSubdirectories = false;
                watcher.Path = InputPath;

                // Only watch dat files.
                watcher.Filter = "*.dat";

                // Add event handlers.
                watcher.Created += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the application.");
                while (Console.Read() != 'q')
                {
                }
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            //ISaleDataProcessor saleDataProcessor = _serviceProvider.GetService<ISaleDataProcessor>();
            //saleDataProcessor.Process(e.FullPath, OutputPath);
            var kafkaProducer = _serviceProvider.GetService<IKafkaProducer>();
            kafkaProducer.SendSale(e.FullPath);

            //TODO colocar no consumer de resposta
            //var outputFilePath = _fileService.GetStatisticsFileName(inputFile, outputPath);
            //_salesSummaryOutputService.Write(outputFilePath, salesSummary);

            //_fileService.MoveProcessedFile(inputFile);

            //Console.WriteLine("File Processed: " + inputFile);
        }

        private static void ProcessExistingFiles(IFileService fileService)
        {
            var kafkaProducer = _serviceProvider.GetService<IKafkaProducer>();

            var unprocessedFiles = fileService.GetUnprocessedFiles(InputPath);
            foreach (var unprocessedFile in unprocessedFiles) kafkaProducer.SendSale(unprocessedFile);
        }
    }
}