using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQ
{
    public class KSender
    {
        public void Send()
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "192.168.2.17:9092" }
            };

            using (var producer = new Producer<string, string>(config, new StringSerializer(Encoding.UTF8), new StringSerializer(Encoding.UTF8)))
            {
                int i = 0;
                var meta = producer.GetMetadata(true, null);
                Console.WriteLine($"{meta.OriginatingBrokerId} {meta.OriginatingBrokerName}");
                meta.Brokers.ForEach(broker =>
                Console.WriteLine($"Broker: {broker.BrokerId} {broker.Host}:{broker.Port}"));
                meta.Topics.ForEach(topic =>
                {
                    Console.WriteLine($"Topic: {topic.Topic} {topic.Error}");
                    topic.Partitions.ForEach(partition =>
                    {
                        Console.WriteLine($"Partition: {partition.PartitionId}");
                        Console.WriteLine($"Replicas: {partition.Replicas}");
                        Console.WriteLine($"InSyncReplicas: {partition.InSyncReplicas}");
                    });
                });



                while (true)
                {
                    var dr = producer.ProduceAsync("merchants-template", null, i.ToString()).Result;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"发送数据 '{dr.Value}' 到: {dr.TopicPartitionOffset}");
                    i++;
                    Thread.Sleep(400);
                }

            }

        }
    }
}
