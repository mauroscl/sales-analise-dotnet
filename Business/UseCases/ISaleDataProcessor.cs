namespace Business.UseCases
{
    public interface ISaleDataProcessor
    {
        void Process(string inputFile, string outputPath);
    }
}
