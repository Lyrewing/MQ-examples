using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MQ
{
    public class Sender
    {
        public void Send()
        {

            using (IConnection con = RabbitMQFactory.CreateConnection())//创建连接对象
            {
                using (IModel channel = con.CreateModel())//创建连接会话对象
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
                    int i = 0;
                    while (true)
                    {
                        String message = i.ToString();
                        //消息内容
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        //发送消息
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("成功发送消息:" + message);
                        Thread.Sleep(500);
                        i++;
                    }
                }
            }
        }













    }
    }

