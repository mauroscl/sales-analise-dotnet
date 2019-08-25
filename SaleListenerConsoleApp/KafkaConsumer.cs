using System;
using System.Linq;
using System.Text;
using System.Threading;
using Business.UseCases;
using Confluent.Kafka;

namespace SaleListenerConsoleApp
{
    internal class KafkaConsumer : IKafkaConsumer
    {
        private readonly ISaleDataProcessor _saleDataProcessor;

        public KafkaConsumer(ISaleDataProcessor saleDataProcessor)
        {
            _saleDataProcessor = saleDataProcessor;
        }

        public void Consume()
        {
            var consumerConfig = new ConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                BootstrapServers = "localhost:9092",
                GroupId = "file-input-consumer"
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumer.Subscribe("test-topic");

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
                            var fileNameHeader = cr.Headers.First(h => h.Key.Equals("file_name"));
                            var fileName = Encoding.ASCII.GetString(fileNameHeader.GetValueBytes());
                            Console.WriteLine(
                                $"Consumed message '{cr.Value}' with header 'file_name' containing '{fileName}' at: '{cr.TopicPartitionOffset}'.");
                            var salesSummary = _saleDataProcessor.Process(cr.Value, "");
                            Console.WriteLine(salesSummary);
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
    }
}