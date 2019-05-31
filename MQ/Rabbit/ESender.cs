using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MQ
{
    public class ESender
    {
        private string exchange;
        private string routingKey;
        public ESender(string exchange,string routingKey)
        {
            this.exchange = exchange;
            this.routingKey = routingKey;
        }
        public void Send()
        {
            using (IConnection con = RabbitMQFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
                {
                    channel.ExchangeDeclare(exchange: exchange, type: "direct");
                    int i = 0;
                    while (true)
                    {
                        String message = i.ToString();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("成功发送消息:" + message);
                        Thread.Sleep(200);
                        i++;
                    }
                }
            }
        }













    }
}

