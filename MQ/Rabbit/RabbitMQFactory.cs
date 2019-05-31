using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQ
{
    public class RabbitMQFactory
    {
        public static IConnection CreateConnection()
        {
            IConnectionFactory conFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "192.168.1.241",//IP地址
                Port = 32769,//端口号
                UserName = "user",//用户账号
                Password = "password"//用户密码
            };
            return conFactory.CreateConnection();
        }
    }
}
