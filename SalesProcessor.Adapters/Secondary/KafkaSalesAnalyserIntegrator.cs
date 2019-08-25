using Confluent.Kafka;
using SalesProcessor.Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesProcessor.Adapters.Secondary
{
    public class KafkaSalesAnalyserIntegrator : ISalesAnalyserIntegrator
    {
        private static readonly string SaleAnalysisInputTopic = "sales-analysis-input";

        private readonly ProducerConfig _producerConfig;

        public KafkaSalesAnalyserIntegrator()
        {
            _producerConfig = new ProducerConfig {BootstrapServers = "localhost:9092"};
        }

        public void SendSaleData(string data, IReadOnlyDictionary<string, string> headers)
        {

            var kakfaHeaders = new Headers();

            foreach (var kafkaHeader in headers.Select(h => new Header(h.Key, Encoding.ASCII.GetBytes(h.Value))))
            {
                kakfaHeaders.Add(kafkaHeader);
            }

            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {

                producer.Produce(SaleAnalysisInputTopic,
                    new Message<Null, string> {Value = data, Headers = kakfaHeaders});
                producer.Flush(TimeSpan.FromSeconds(5));

                headers.TryGetValue("CTM_FILE_NAME", out var fileName);
                Console.WriteLine($"Content of file {fileName} sent to kafka");
            }
        }
    }
}