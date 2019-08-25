using System;
using System.Collections.Generic;
using System.IO;

namespace InfraStructure
{
    public class FileService : IFileService
    {
        private const string ProcessedDirectory = "processed";

        public IList<string> GetUnprocessedFiles(string inputDirectory)
        {
            return Directory.GetFiles(inputDirectory, "*.dat");
        }

        public void CreateApplicationDirectories(string inputPath, string outputPath)
        {
            Directory.CreateDirectory(Path.Combine(inputPath, ProcessedDirectory));
            Directory.CreateDirectory(outputPath);
        }

        public void MoveProcessedFile(string inputFile)
        {
            var destinationProcessedFile = Path.Combine(Path.GetDirectoryName(inputFile), ProcessedDirectory,
                Path.GetFileName(inputFile));

            try
            {
                File.Move(inputFile, destinationProcessedFile);
            }
            catch (IOException e)
            {
                Console.WriteLine("Can't move the file to processed destination folder. Input file: " + inputFile);
                Console.WriteLine(e.Message);
            }
        }

        public string GetStatisticsFileName(string inputFile, string outputPath)
        {
            var destinationFileName =
                Path.GetFileNameWithoutExtension(inputFile) + ".done" + Path.GetExtension(inputFile);
            return Path.Combine(outputPath, destinationFileName);
        }
    }
}