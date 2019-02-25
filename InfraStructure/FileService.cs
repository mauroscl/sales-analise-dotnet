using System;
using System.Collections.Generic;
using System.IO;
using Business;

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
    }
}