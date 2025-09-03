namespace Lamport
{
    // Clase que representa un proceso con un reloj lógico de Lamport
    public class LamportProcess
    {
        public int Clock { get; private set; }
        public string Name { get; }

        public LamportProcess(string name)
        {
            Name = name;
            Clock = 0;
        }

        // Evento interno (incrementa el reloj)
        public void InternalEvent()
        {
            Clock++;
            Console.WriteLine($"{Name} realizó evento interno. Reloj: {Clock}");
        }

        // Enviar mensaje (incrementa el reloj y retorna el valor enviado)
        public int SendMessage()
        {
            Clock++;
            Console.WriteLine($"{Name} envió mensaje. Reloj: {Clock}");
            return Clock;
        }

        // Recibir mensaje (ajusta el reloj según el valor recibido)
        public void ReceiveMessage(int receivedClock)
        {
            Clock = Math.Max(Clock, receivedClock) + 1;
            Console.WriteLine($"{Name} recibió mensaje. Reloj actualizado: {Clock}");
        }
    }

    public class LamportLogicalClock
    {
        public static void Run()
        {
            var p1 = new LamportProcess("P1");
            var p2 = new LamportProcess("P2");

            // Simulación de eventos y mensajes
            p1.InternalEvent(); // P1: 1
            int msg1 = p1.SendMessage(); // P1: 2
            Thread.Sleep(500);

            p2.ReceiveMessage(msg1); // P2: 3
            p2.InternalEvent(); // P2: 4
            int msg2 = p2.SendMessage(); // P2: 5
            Thread.Sleep(500);

            p1.ReceiveMessage(msg2); // P1: 6

            Console.WriteLine($"Estado final: {p1.Name}={p1.Clock}, {p2.Name}={p2.Clock}");
        }
    }
}