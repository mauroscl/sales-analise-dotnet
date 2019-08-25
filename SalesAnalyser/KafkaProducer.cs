using System;
using System.IO;
using System.Text;
using Confluent.Kafka;

namespace SalesAnalyser
{
    public interface IKafkaProducer
    {
        void SendSale(string filePath);
    }

    internal class KafkaProducer : IKafkaProducer
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducer()
        {
            _producerConfig = new ProducerConfig {BootstrapServers = "localhost:9092"};
        }

        public void SendSale(string filePath)
        {
            var saleContent = File.ReadAllText(filePath);
            var fileName = Path.GetFileName(filePath);

            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                var headers = new Headers {{"file_name", Encoding.ASCII.GetBytes(fileName)}};

                producer.Produce("test-topic", new Message<Null, string> {Value = saleContent, Headers = headers});
                producer.Flush(TimeSpan.FromSeconds(5));
            }
        }


        public void SendMessages()
        {
            using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
            {
                producer.Produce("test-topic", new Message<Null, string> {Value = "mensagem de teste"});
                producer.Flush(TimeSpan.FromSeconds(5));
            }
        }
    }
}