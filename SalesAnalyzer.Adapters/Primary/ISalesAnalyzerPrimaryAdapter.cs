using System.Threading;

namespace SalesAnalyzer.Adapters.Primary
{
    public interface ISalesAnalyzerPrimaryAdapter
    {
        void ConfigureConsumer(CancellationToken cancellationToken);
    }
}