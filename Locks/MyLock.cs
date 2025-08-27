namespace Concurrency.Threadsafe
{
    public static class MyLock
    {
        private static int counter = 0;
        private static readonly object counterLock = new();

        public static void Run()
        {
            const int threadCount = 50;
            const int incrementsPerThread = 10000;
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < incrementsPerThread; j++)
                    {
                        // Sección crítica protegida por lock
                        lock (counterLock)
                        {
                            int temp = counter;
                            Thread.SpinWait(20);
                            counter = temp + 1;
                        }
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Ejemplo con lock");
            Console.WriteLine($"Esperado: {threadCount * incrementsPerThread}");
            Console.WriteLine($"Actual:   {counter}");
        }
    }
}