using System;
using System.Threading;

namespace Lamport
{
    public class LamportThreeProcesses
    {
        public static void Run()
        {
            var pA = new LamportProcess("A");
            var pB = new LamportProcess("B");
            var pC = new LamportProcess("C");

            // A y B son concurrentes, ambos se comunican con C

            // Paso 1: A y B hacen eventos internos concurrentes
            pA.InternalEvent(); // A:1
            pB.InternalEvent(); // B:1

            // Paso 2: A y B envían mensajes a C (simultáneamente)
            int msgA = pA.SendMessage(); // A:2
            int msgB = pB.SendMessage(); // B:2

            // Simula que C recibe ambos mensajes (el orden puede variar, aquí primero A luego B)
            pC.ReceiveMessage(msgA); // C:3
            pC.ReceiveMessage(msgB); // C:4

            // Paso 3: C responde a A y B
            int msgCtoA = pC.SendMessage(); // C:5
            int msgCtoB = pC.SendMessage(); // C:6

            // A y B reciben la respuesta de C
            pA.ReceiveMessage(msgCtoA); // A:6 (max(2,5)+1)
            pB.ReceiveMessage(msgCtoB); // B:7 (max(2,6)+1)

            // Paso 4: A y B hacen eventos internos concurrentes
            pA.InternalEvent(); // A:7
            pB.InternalEvent(); // B:8

            Console.WriteLine();
            Console.WriteLine($"Estado final: {pA.Name}={pA.Clock}, {pB.Name}={pB.Clock}, {pC.Name}={pC.Clock}");
        }
    }
}