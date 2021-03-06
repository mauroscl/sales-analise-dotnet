﻿using Confluent.Kafka;
using SalesProcessor.Application.Ports.Driven;
using System;
using System.Text;
using SalesProcessor.Configuration;

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
                BootstrapServers = Environment.GetEnvironmentVariable(AppConfig.KafkaServerEnv)
            };
        }

        public void SendSaleData(string data, string saleKey)
        {
            
            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                var kafkaHeaders = new Headers
                {
                    {AppConfig.KafkaFileNameHeader, Encoding.ASCII.GetBytes(saleKey)},
                    {AppConfig.KafkaSalesAnalysisOutputTopicHeader, Encoding.ASCII.GetBytes(AppConfig.GetSalesAnalysisOutputTopic())}
                };

                producer.Produce(SaleAnalysisInputTopic,
                    new Message<string, string> {Key = saleKey , Value = data, Headers = kafkaHeaders });
                producer.Flush(TimeSpan.FromSeconds(30));

                Console.WriteLine($"Content of file {saleKey} sent to kafka");
            }
        }
    }
}