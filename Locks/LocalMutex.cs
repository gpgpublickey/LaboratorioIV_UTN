using System;
using System.Threading;

namespace Concurrency.Threadsafe
{
    public class LocalMutex
    {
        
        public static void Run()
        {
            int counter = 0;
            Mutex mutex = new Mutex();
            const int threadCount = 10;
            const int incrementsPerThread = 10000;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < incrementsPerThread; j++)
                    {
                        mutex.WaitOne();
                        int temp = counter;
                        Thread.SpinWait(20);
                        counter = temp + 1;
                        mutex.ReleaseMutex();
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Ejemplo con Mutex local");
            Console.WriteLine($"Esperado: {threadCount * incrementsPerThread}");
            Console.WriteLine($"Actual:   {counter}");
        }
    }
}