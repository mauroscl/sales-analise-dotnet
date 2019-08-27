namespace SalesAnalyzer.Adapters.Secondary
{
    public interface IFileHelperEngine
    {
        object[] ReadCsv(string content);
    }
}