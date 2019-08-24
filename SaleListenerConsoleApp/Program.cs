using System;

namespace SaleListenerConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer - Hello World!");
            KafkaConsumer.Consume();
        }

    }
}
