using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Confluent.Kafka;

namespace SaleListenerConsoleApp
{
    class KafkaConsumer
    {
        public static void Consume()
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

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
                            var fileNameHeader = cr.Headers.First(h => h.Key.Equals("file_name"));
                            var fileName = Encoding.ASCII.GetString(fileNameHeader.GetValueBytes());
                            Console.WriteLine($"Consumed message '{cr.Value}' with header 'file_name' containing '{fileName}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
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
