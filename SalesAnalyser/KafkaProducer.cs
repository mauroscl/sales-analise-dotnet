using System;
using System.Text;
using Confluent.Kafka;

namespace SalesAnalyser
{
    class KafkaProducer
    {


        public static void SendMessages()
        {
            var producerConfig = new ProducerConfig {BootstrapServers = "localhost:9092"};
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                producer.Produce("test-topic", new Message<Null, string>{Value = "mensagem de teste"});
                producer.Flush(TimeSpan.FromSeconds(5));
            }
        }

        public static void SendSale()
        {
            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                var saleContent =
                    "001ç1234567891234çDiegoç50000\r\n001ç3245678865434çRenatoç40000.99\r\n002ç2345675434544345çJose da SilvaçRural\r\n002ç2345675433444345çEduardo PereiraçRural\r\n003ç10ç[1-10-100,2-30-2.50,3-40-3.10]çDiego\r\n003ç08ç[1-34-10,2-33-1.50,3-40-0.10]çRenato";

                var headers = new Headers {{"file_name", Encoding.ASCII.GetBytes("venda.txt")}};

                producer.Produce("test-topic", new Message<Null, string> { Value = saleContent, Headers = headers});
                producer.Flush(TimeSpan.FromSeconds(5));
            }
        }
    }
}
