using Confluent.Kafka;
using SalesProcessor.Application.Ports.Driven;
using System;
using System.Text;
using SalesProcessor.Adapters.Primary;

namespace SalesProcessor.Adapters.Secondary
{
    public class KafkaSalesAnalyserIntegrator : ISalesAnalyserIntegrator
    {
        private static readonly string SaleAnalysisInputTopic = "sales-analysis-input";

        private readonly ProducerConfig _producerConfig;

        public KafkaSalesAnalyserIntegrator()
        {
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVER")
            };
        }

        public void SendSaleData(string data, string saleKey)
        {
            
            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                var kafkaHeaders = new Headers {{KafkaConfig.FileNameHeader, Encoding.ASCII.GetBytes(saleKey)}};

                producer.Produce(SaleAnalysisInputTopic,
                    new Message<string, string> {Key = saleKey , Value = data, Headers = kafkaHeaders }, report =>
                    {
                        if (report.Error != null)
                        {
                            Console.WriteLine(report.Error.Reason);
                        }
                    });
                producer.Flush(TimeSpan.FromSeconds(30));

                Console.WriteLine($"Content of file {saleKey} sent to kafka");
            }
        }
    }
}