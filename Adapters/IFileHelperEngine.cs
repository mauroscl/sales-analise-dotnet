namespace Adapters
{
    public interface IFileHelperEngine
    {
        object[] ReadCsvFile(string filePath);
        object[] ReadCsv(string content);
    }
}