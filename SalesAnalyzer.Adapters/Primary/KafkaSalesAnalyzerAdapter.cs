using Confluent.Kafka;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports.Driver;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace SalesAnalyzer.Adapters.Primary
{
    public class KafkaSalesAnalyzerAdapter : ISalesAnalyzerPrimaryAdapter
    {

        private const string SaleAnalysisInputTopic = "sales-analysis-input";

        private const string ConsumerGroup = "sales-data-consumer";

        private const string CustomHeadersPrefix = "CTM";
        private static readonly string KafkaServer = Environment.GetEnvironmentVariable("KAFKA_SERVER");

        private readonly ISaleDataProcessor _saleDataProcessor;

        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;

        public KafkaSalesAnalyzerAdapter(ISaleDataProcessor saleDataProcessor)
        {
            _saleDataProcessor = saleDataProcessor;

            _consumerConfig  = new ConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = KafkaServer,
                GroupId = ConsumerGroup
            };

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = KafkaServer
            };

        }

        public void ConfigureConsumer(CancellationToken cancellationToken)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(SaleAnalysisInputTopic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                        try
                        {
                            var cr = consumer.Consume(cancellationToken);
                            var salesSummary = _saleDataProcessor.Process(cr.Value);
                            SendResponse(salesSummary, cr.Headers);
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

        private void SendResponse(SalesSummary salesSummary, Headers headers)
        {
            var customHeaders = CopyCustomHeaders(headers);

            var header = customHeaders.First(h => h.Key.Equals("CTM_SALES_ANALYSIS_OUTPUT_TOPIC"));
            var outputTopic = Encoding.ASCII.GetString(header.GetValueBytes());

            var serializeSaleSummary = JsonConvert.SerializeObject(salesSummary, Formatting.Indented);

            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {

                producer.Produce(outputTopic,new Message<Null, string> { Value = serializeSaleSummary, Headers = customHeaders });

                producer.Flush(TimeSpan.FromSeconds(5));

                PrintLogs(customHeaders);

            }

        }

        private Headers CopyCustomHeaders(Headers headers)
        {
            var customHeaders = headers.Where(h => h.Key.StartsWith(CustomHeadersPrefix));
            var kafkaHeaders = new Headers();
            foreach (var customHeader in customHeaders)
            {
                kafkaHeaders.Add(customHeader.Key, customHeader.GetValueBytes());
            }

            return kafkaHeaders;
        }

        private void PrintLogs(Headers headers)
        {
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Message processed");
            Console.WriteLine("Custom Headers:");
            foreach (var header in headers)
            {
                Console.WriteLine($"{header.Key}: {Encoding.ASCII.GetString(header.GetValueBytes())}");
            }
            Console.WriteLine("-------------------------------------------");

        }

    }
}