using Business;
using InfraStructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace SalesAnalyser
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private const string InputPath = "d:\\data\\in";
        private const string OutputPath = "d:\\data\\out";

        static void Main(string[] args)
        {
            _serviceProvider = new ServiceCollection()
                .AddTransient<ISaleDataProcessor, SaleCsvProcessor>()
                .AddTransient<ISalesContextLoader, SalesContextLoader>()
                .AddTransient<ISalesStatisticsService, SalesStatisticsService>()
                .AddTransient<ISalesSummaryOutputService, SalesSummaryOutputFileService>()
                .AddTransient<IFileService, FileService>()
                .BuildServiceProvider();

            Run();
        }

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {
            string[] args = Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
            //if (args.Length != 2)
            //{
            //    // Display the proper way to call the program.
            //    Console.WriteLine("Usage: Watcher.exe (directory)");
            //    return;
            //}

            var fileService = _serviceProvider.GetService<IFileService>();

            fileService.CreateApplicationDirectories(InputPath, OutputPath);

            ISaleDataProcessor saleDataProcessor = _serviceProvider.GetService<ISaleDataProcessor>();

            var unprocessedFiles = fileService.GetUnprocessedFiles(InputPath);
            foreach (var unprocessedFile in unprocessedFiles)
            {
                saleDataProcessor.Process(unprocessedFile, OutputPath);
            }

            InitializeWatcher();

        }

        private static void InitializeWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.IncludeSubdirectories = false;
                watcher.Path = InputPath;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                       | NotifyFilters.LastWrite
                                       | NotifyFilters.FileName;

                // Only watch dat files.
                watcher.Filter = "*.dat";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                //watcher.Deleted += OnChanged;
                //watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the application.");
                while (Console.Read() != 'q') ;

            }


        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            ISaleDataProcessor saleDataProcessor = _serviceProvider.GetService<ISaleDataProcessor>();
            saleDataProcessor.Process(e.FullPath, OutputPath);
        }
    }
}
