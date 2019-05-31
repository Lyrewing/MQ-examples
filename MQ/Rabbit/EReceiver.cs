using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MQ
{
    public class EReceiver
    {
        private string name;
        private int time;
        private string exchange;
        private string routekey;
        public EReceiver(string exchange, string name,string routekey,int time)
        {
            this.name = name;
            this.time = time;
            this.exchange = exchange;
            this.routekey = routekey;
        }
        public void Receive()
        {
            int random = new Random().Next(1, 1000);
            using (IConnection conn = RabbitMQFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //交换机名称
                    String exchangeName = exchange;
                    //声明交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct");
                    //channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
                    //消息队列名称
                    String queueName = exchangeName + "_" + random.ToString();
                    //声明队列
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    //将队列与交换机进行绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routekey);
                    //声明为手动确认
                    channel.BasicQos(0, 1, false);
                    //定义消费者
                    var consumer = new EventingBasicConsumer(channel);
                    //接收事件
                    consumer.Received += (model, ea) =>
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        byte[] message = ea.Body;//接收到的消息
                        Console.WriteLine($"{name},{queueName}接收到信息为:" + Encoding.UTF8.GetString(message));
                        //返回消息确认
                        channel.BasicAck(ea.DeliveryTag, true);
                        Thread.Sleep(time);
                    };
                    //开启监听
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
                    Console.ReadKey();
                }
            }









        }
    }
}
