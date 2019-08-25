using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using SalesAnalyzer.Application.Domain;
using SalesAnalyzer.Application.Ports.Driver;

namespace SalesAnalyzer.Adapters.Primary
{
    public class KafkaSalesAnalyzerAdapter : ISalesAnalyzerPrimaryAdapter
    {

        private static readonly string SaleAnalysisInputTopic = "sales-analysis-input";
        private static readonly string SaleAnalysisOutputTopic = "sales-analysis-output";

        private static readonly string ConsumerGroup = "file-input-consumer";

        private static readonly string CustomHeadersPrefix = "CTM";
        private static readonly string KafkaServer = "localhost:9092";

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

        public void ConfigureConsumer()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(SaleAnalysisInputTopic);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
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

            

            var producerBuilder = new ProducerBuilder<Null, SalesSummary>(_producerConfig);
            producerBuilder.SetValueSerializer(Serializers.ByteArray);
            using (var producer = producerBuilder.Build())
            {


                producer.Produce(SaleAnalysisOutputTopic,
                    new Message<Null, SalesSummary> { Value = salesSummary, Headers = customHeaders });
                producer.Flush(TimeSpan.FromSeconds(5));

                var fileName = Encoding.ASCII.GetString(headers.FirstOrDefault(h => h.Key.Equals("CTM_FILE_NAME"))?.GetValueBytes()) ;

                Console.WriteLine($"Response for file {fileName} sent to kafka");
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

    }
}