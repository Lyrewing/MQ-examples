using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ
{
    public class KConsumer
    {
        public void Receive()
        {
            var conf = new Dictionary<string, object>
            {
              { "group.id", "test-consumer-group" },
              { "bootstrap.servers", "master:9092" },
              { "auto.commit.interval.ms", 5000 },
              { "auto.offset.reset", "earliest" }
            };


            using (var consumer = new Consumer<string, string>(conf,new StringDeserializer(Encoding.UTF8), new StringDeserializer(Encoding.UTF8)))
            {

                consumer.OnMessage += (_, msg)
                  =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"读取 '{msg.Value}' 从: {msg.TopicPartitionOffset}");
                };

                consumer.OnError += (_, error)
                  => Console.WriteLine($"Error: {error}");

                consumer.OnConsumeError += (_, msg)
                  => Console.WriteLine($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe("merchants-template");
                while (true)
                {
                    consumer.Poll(TimeSpan.FromMilliseconds(100));
                }
            }

            

        }
    }
}
