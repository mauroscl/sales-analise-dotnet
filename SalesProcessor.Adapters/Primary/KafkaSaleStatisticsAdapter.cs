using Confluent.Kafka;
using SalesProcessor.Application.Ports.Driver;
using SalesProcessor.Application.UseCases;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SalesProcessor.Adapters.Primary
{
    public class KafkaSaleStatisticsAdapter
    {

        private static readonly string SaleAnalysisOutputTopic = "sales-analysis-output";

        private static readonly string ConsumerGroup = "sales-statistics-consumer";

        private static readonly string KafkaServer = "localhost:9092";

        private readonly ConsumerConfig _consumerConfig;

        private readonly ISaleStatisticsProcessor _saleStatisticsProcessor;
        public KafkaSaleStatisticsAdapter(ISaleStatisticsProcessor saleStatisticsProcessor)
        {
            _saleStatisticsProcessor = saleStatisticsProcessor;
            _consumerConfig = new ConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = KafkaServer,
                GroupId = ConsumerGroup
            };
        }

        public void ConfigureConsumer(string fileInputPath, string fileOutputPath, CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(SaleAnalysisOutputTopic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                        try
                        {
                            var consumerRecord = consumer.Consume(cancellationToken);
                            var fileName = GetFileName(consumerRecord.Headers);

                            var salesSummary = JsonConvert.DeserializeObject<SalesSummary>(consumerRecord.Value);

                            _saleStatisticsProcessor.PersistStatistics(salesSummary, fileName, fileInputPath, fileOutputPath);

                            Console.WriteLine($"File {fileName} processed");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    consumer.Close();
                }
            }
        }

        private string GetFileName(Headers headers)
        {
            var header = headers.First(h => h.Key.Equals(KafkaConfig.FileNameHeader));
            return Encoding.ASCII.GetString(header.GetValueBytes());
        }

    }
}
