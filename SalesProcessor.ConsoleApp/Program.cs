using InfraStructure;
using Microsoft.Extensions.DependencyInjection;
using SalesProcessor.Adapters.Primary;
using SalesProcessor.Adapters.Secondary;
using SalesProcessor.Application.Ports.Driven;
using SalesProcessor.Application.Ports.Driver;
using SalesProcessor.Application.UseCases;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SalesProcessor.Configuration;

namespace SalesProcessor.ConsoleApp
{
    internal static class Program
    {
        //private const string InputPath = "d:\\data\\in";
        //private const string OutputPath = "d:\\data\\out";

        //private const string InputPath = "data/in";
        //private const string OutputPath = "data/out";

        private static IServiceProvider _serviceProvider;

        private static void Main(string[] args)
        {

            Console.WriteLine("Running Sales Processor...");

            if (args.Length != 3)
            {
                Console.WriteLine("Invalid number of arguments");
                Console.WriteLine("Usage: dotnet run <KAFKA_SERVER> <INPUT_PATH> <OUTPUT_PATH>");
                Environment.Exit(1);
            }

            var applicationId = GenerateApplicationId(args[1], args[2]);

            Environment.SetEnvironmentVariable(AppConfig.KafkaServerEnv, args[0]);
            Environment.SetEnvironmentVariable(AppConfig.InputPathEnv, args[1]);
            Environment.SetEnvironmentVariable(AppConfig.OuputPathEnv, args[2]);
            Environment.SetEnvironmentVariable(AppConfig.ApplicationIdEnv, applicationId);

            var fileInputPath = Environment.GetEnvironmentVariable(AppConfig.InputPathEnv);
            var fileOutputPath = Environment.GetEnvironmentVariable(AppConfig.OuputPathEnv);

            Console.WriteLine("ENVIRONMENT...");
            Console.WriteLine($"APPLICATION IDENTIFIER: {Environment.GetEnvironmentVariable(AppConfig.ApplicationIdEnv)}");
            Console.WriteLine($"KAFKA SERVER: {args[0]}");
            Console.WriteLine($"INPUT PATH: {fileInputPath}");
            Console.WriteLine($"OUTPUT PATH: {fileOutputPath}");

            RegistryServices();

            var fileService = _serviceProvider.GetService<IFileService>();
            fileService.CreateApplicationDirectories(fileInputPath,
                fileOutputPath);

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };


            var kafkaSaleOutputAdapter = _serviceProvider.GetService<KafkaSaleStatisticsAdapter>();
            Task.Run(() => kafkaSaleOutputAdapter.ConfigureConsumer(fileInputPath
                , fileOutputPath, cts.Token), cts.Token);

            ProcessExistingFiles(fileInputPath, fileService);

            InitializeFileWatcher(fileInputPath, cts.Token);
        }

        private static void RegistryServices()
        {
            _serviceProvider = new ServiceCollection()

                .AddTransient<IFileService, FileService>()
                .AddTransient<IFileNameService, FileNameService>()
                .AddTransient<ISalesSummaryOutputService, SalesSummaryOutputFileService>()
                .AddTransient<ISalesAnalyserIntegrator, KafkaSalesAnalyserIntegrator>()
                .AddTransient<ISaleDataProcessor, SaleDataProcessor>()
                .AddTransient<ISaleStatisticsProcessor, SaleStatisticsFileProcessor>()
                .AddTransient<FileSaleDataAdapter, FileSaleDataAdapter>()
                .AddTransient<KafkaSaleStatisticsAdapter, KafkaSaleStatisticsAdapter>()
                .BuildServiceProvider();
        }


        private static void InitializeFileWatcher(string fileInputPath, CancellationToken cancellationToken)
        {
            // Create a new FileSystemWatcher and set its properties.
            using (var watcher = new FileSystemWatcher())
            {
                watcher.IncludeSubdirectories = false;
                watcher.Path = fileInputPath;

                // Only watch dat files.
                watcher.Filter = "*.dat";

                // Add event handlers.
                watcher.Created += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

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
        }

        private static void ProcessExistingFiles(string fileInputPath, IFileService fileService)
        {
            var fileSaleProcessor = _serviceProvider.GetService<FileSaleDataAdapter>();

            var unprocessedFiles = fileService.GetUnprocessedFiles(fileInputPath);
            foreach (var unprocessedFile in unprocessedFiles)
            {
                fileSaleProcessor.ProcessFile(unprocessedFile);
            }
        }

        private static string GenerateApplicationId(string inputPath, string outputPath)
        {
            var stringToConvert = Path.GetFullPath(inputPath) + Path.GetFullPath(outputPath) +  Environment.MachineName ;
            var inputBuffer = Encoding.UTF8.GetBytes(stringToConvert);

            var base64String = Convert.ToBase64String(inputBuffer);
            return base64String.Substring(0, 10) + base64String.Substring(base64String.Length -10, 10);

        }
    }
}