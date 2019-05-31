using System;
using System.Collections.Generic;
using System.Text;

namespace MQ
{
    public class Publisher
    {
        private int value;
        public delegate void NumManipulationHandler();
        public event NumManipulationHandler ChangeNum;
        protected virtual void OnNumChanged()
        {
            if (ChangeNum != null)
            {
                ChangeNum(); /* 事件被触发 */
            }
            else
            {
                Console.WriteLine("event not fire");
                Console.ReadKey(); /* 回车继续 */
            }
        }

        public void SetValue(int n)
        {
            if (value != n)
            {
                value = n;
                OnNumChanged();
            }
        }

    }



    /***********订阅器类***********/

    public class Subscriber
    {
        public void printf()
        {
            Console.WriteLine("event fire");
            Console.ReadKey(); /* 回车继续 */
        }
    }
















}
