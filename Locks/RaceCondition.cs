namespace Concurrency.Threadsafe
{
    public class RaceCondition
    {
        private static int counter = 0;

        public static void Run()
        {
            const int threadCount = 50; // Más hilos
            const int incrementsPerThread = 10000; // Más repeticiones
            Thread[] threads = new Thread[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < incrementsPerThread; j++)
                    {
                        // Pequeño retardo para aumentar la probabilidad de colisión
                        int temp = counter;
                        Thread.SpinWait(20); // Espera activa muy corta
                        counter = temp + 1;
                    }
                });
                threads[i].Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("Ejemplo de problema de condicion de carrera");
            Console.WriteLine($"Esperado: {threadCount * incrementsPerThread}");
            Console.WriteLine($"Actual:   {counter}");
        }
    }
}
