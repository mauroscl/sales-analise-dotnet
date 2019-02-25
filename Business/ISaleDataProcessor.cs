namespace Business
{
    public interface ISaleDataProcessor
    {
        void Process(string sourceFile, string destinationPath);
    }
}
