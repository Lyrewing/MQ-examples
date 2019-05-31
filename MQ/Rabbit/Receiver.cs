using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MQ
{
    public class Receiver
    {
        private string name;
        private int time;
        public Receiver(string name, int time)
        {
            this.name = name;
            this.time = time;

        }
        public void Receive()
        {
            using (IConnection conn = RabbitMQFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    String queueName = "queue1";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: false,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        Thread.Sleep(time);//随机等待,实现能者多劳,;//等待1秒,
                        byte[] message = ea.Body;//接收到的消息
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{name}接收到信息为:" + Encoding.UTF8.GetString(message));
                        channel.BasicAck(ea.DeliveryTag, true);
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    channel.BasicQos(0, 1, false);
                    Console.ReadKey();
                }
            }
        }
    }
}
