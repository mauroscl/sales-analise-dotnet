using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using InfraStructure;
using Microsoft.Extensions.DependencyInjection;
using SalesProcessor.Adapters.Primary;
using SalesProcessor.Adapters.Secondary;
using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;
using SalesProcessor.Application.UseCases;

namespace SalesProcessor.ConsoleApp
{
    internal static class Program
    {
        //private const string InputPath = "d:\\data\\in";
        //private const string OutputPath = "d:\\data\\out";

        private const string InputPath = "data/in";
        private const string OutputPath = "data/out";
        private static IServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {

            Console.WriteLine("Running Sales Processor...");

            RegistryServices();

            var fileService = _serviceProvider.GetService<IFileService>();
            fileService.CreateApplicationDirectories(InputPath, OutputPath);

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };


            var kafkaSaleOutputAdapter = _serviceProvider.GetService<KafkaSaleStatisticsAdapter>();
            Task.Run(() => kafkaSaleOutputAdapter.ConfigureConsumer(InputPath, OutputPath, cts.Token), cts.Token);

            ProcessExistingFiles(fileService);

            InitializeFileWatcher(cts.Token);
        }

        private static void RegistryServices()
        {
            _serviceProvider = new ServiceCollection()

                .AddTransient<IFileService, FileService>()
                .AddTransient<ISalesSummaryOutputService, SalesSummaryOutputFileService>()
                .AddTransient<ISalesAnalyserIntegrator, KafkaSalesAnalyserIntegrator>()
                .AddTransient<ISaleDataProcessor, SaleDataProcessor>()
                .AddTransient<ISaleStatisticsProcessor, SaleStatisticsFileProcessor>()
                .AddTransient<FileSaleDataAdapter, FileSaleDataAdapter>()
                .AddTransient<KafkaSaleStatisticsAdapter, KafkaSaleStatisticsAdapter>()
                .BuildServiceProvider();
        }


        private static void InitializeFileWatcher(CancellationToken cancellationToken)
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

                //// Wait for the user to quit the program.
                //Console.WriteLine("Press 'q' to quit the application.");
                //while (Console.Read() != 'q')
                //{

                //}

                Console.WriteLine($"Watching folder {Path.GetFullPath(watcher.Path)}");

                while (!cancellationToken.IsCancellationRequested) ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            var fileSaleProcessor = _serviceProvider.GetService<FileSaleDataAdapter>();
            fileSaleProcessor.ProcessFile(e.FullPath);


            //Console.WriteLine("File Processed: " + inputFile);
        }

        private static void ProcessExistingFiles(IFileService fileService)
        {
            var fileSaleProcessor = _serviceProvider.GetService<FileSaleDataAdapter>();

            var unprocessedFiles = fileService.GetUnprocessedFiles(InputPath);
            foreach (var unprocessedFile in unprocessedFiles)
            {
                fileSaleProcessor.ProcessFile(unprocessedFile);
            }
        }
    }
}