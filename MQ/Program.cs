using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MQ
{

    class Persion : Human
    {
        public string Name { get; set; }

    }

    class Human
    {
        public int Age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            //Receiver receiver1 = new Receiver("receiver 1",2000);
            //Receiver receiver2 = new Receiver("receiver 2",200);
            //Sender sender = new Sender();
            //Task.Run(()=> {
            //    sender.Send();
            //});

            //Task.Run(()=> {
            //    receiver1.Receive();
            //});

            //Task.Run(() => {
            //    receiver2.Receive();
            //});



            //EReceiver receiver1 = new EReceiver("exchange2", "receiver 1","order", 1000);
            //EReceiver receiver2 = new EReceiver("exchange2", "receiver 2","payment", 500);
            //ESender sender = new ESender("exchange2", "order");
            //ESender sender2 = new ESender("exchange2", "payment");
            //Task.Run(() =>{ sender.Send();});
            //Task.Run(() =>{sender2.Send(); });
            //Task.Run(() =>{receiver1.Receive();});
            //Task.Run(() =>{receiver2.Receive();});

            KSender sender = new KSender();
            KConsumer consumer = new KConsumer();

            Task.Run(() =>
            {
                sender.Send();
            });

            Task.Run(() =>
            {
                consumer.Receive();
            });




            //Publisher publisher = new Publisher();
            //Subscriber subscriber = new Subscriber();


            //publisher.ChangeNum += new Publisher.NumManipulationHandler(subscriber.printf);

            //publisher.SetValue(1);
            //publisher.SetValue(2);




            Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmss"));
            Console.ReadKey();
        }
    }

    class P {
        public string name { get; set; }
    }
}
