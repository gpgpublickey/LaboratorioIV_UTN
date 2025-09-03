namespace Lamport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Ejecutar ejemplo de reloj logico de Lamport
            LamportLogicalClock.Run();
            Console.WriteLine("\n---------------------------------\n");

            // Ejecutar ejemplo de concurrencia entre procesos con relojes logicos de Lamport
            LamportThreeProcesses.Run();
            Console.WriteLine("\n---------------------------------\n");

            // Ejecutar ejemplo de algoritmo de consenso y lider con lamport consumiendo de zookeeper
            //Task.Run(LamportWithZooKeeper.RunAsync);
            Thread.Sleep(1000);
        }
    }
}
