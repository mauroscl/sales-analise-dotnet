using System;
using System.IO;
using InfraStructure;
using Microsoft.Extensions.DependencyInjection;
using SalesProcessor.Adapters.Primary;
using SalesProcessor.Adapters.Secondary;
using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;
using SalesProcessor.Application.UseCases;

namespace SalesProcessor.ConsoleApp
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

                .AddTransient<IFileService, FileService>()
                .AddTransient<ISalesSummaryOutputService, SalesSummaryOutputFileService>()
                .AddTransient<ISalesAnalyserIntegrator, KafkaSalesAnalyserIntegrator>()
                .AddTransient<ISaleInputProcessor, SaleInputProcessor>()
                .AddTransient<FileSaleInputAdapter, FileSaleInputAdapter>()
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
            var fileSaleProcessor = _serviceProvider.GetService<FileSaleInputAdapter>();
            fileSaleProcessor.ProcessFile(e.FullPath);


            //Console.WriteLine("File Processed: " + inputFile);
        }

        private static void ProcessExistingFiles(IFileService fileService)
        {
            var fileSaleProcessor = _serviceProvider.GetService<FileSaleInputAdapter>();

            var unprocessedFiles = fileService.GetUnprocessedFiles(InputPath);
            foreach (var unprocessedFile in unprocessedFiles)
            {
                fileSaleProcessor.ProcessFile(unprocessedFile);
            }
        }
    }
}