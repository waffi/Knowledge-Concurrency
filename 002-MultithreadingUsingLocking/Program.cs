using System;
using System.Threading;

namespace _002_MultithreadingUsingLocking
{
    class Program
    {
        static int Count = 0;
        static void Main(string[] args)
        {
            Thread t1 = new Thread(IncrementCount);
            Thread t2 = new Thread(IncrementCount);
            Thread t3 = new Thread(IncrementCount);
            t1.Start();
            t2.Start();
            t3.Start();
            //Wait for all three threads to complete their execution
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine(Count);
            Console.Read();
        }
        private static object _lockObject = new object();
        static void IncrementCount()
        {
            for (int i = 1; i <= 1000000; i++)
            {
                //Only protecting the shared Count variable
                lock (_lockObject)
                {
                    Count++;
                }
            }
        }
    }
}
