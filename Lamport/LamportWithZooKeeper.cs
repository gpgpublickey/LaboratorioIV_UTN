using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using org.apache.zookeeper;

namespace Lamport
{
    // Requiere el paquete NuGet: ZooKeeperNetEx (o similar)
    // Este ejemplo es conceptual y asume que ZooKeeper está corriendo en localhost:2181

    public class LamportWithZooKeeper : Watcher
    {
        private static ZooKeeper _zk;
        private int _clock;
        private readonly string _processName;
        private readonly string _zkPath = "/lamport_demo";
        private HashSet<string> _processedEvents = new HashSet<string>();

        public LamportWithZooKeeper(string processName)
        {
            _processName = processName;
            _clock = 0;
        }

        public override async Task process(WatchedEvent @event)
        {
            // Solo nos interesa NodeChildrenChanged en el znode raíz
            if (@event.get_Type() == Event.EventType.NodeChildrenChanged && @event.getPath() == _zkPath)
            {
                await ReceiveMessagesAsync(true);
            }
        }

        public async Task ConnectAsync()
        {
            if (_zk == null)
            {
                _zk = new ZooKeeper("localhost:2181", 3000, this);
                // Crea el nodo raíz si no existe
                if (_zk.existsAsync(_zkPath).Result == null)
                    await _zk.createAsync(_zkPath, new byte[0], ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
            }
            // Registra el watcher inicial
            await WatchForMessagesAsync();
        }

        // Evento interno
        public async Task InternalEventAsync()
        {
            _clock++;
            Console.WriteLine($"{_processName} evento interno. Reloj: {_clock}");
            await PublishEventAsync($"internal:{_clock}");
        }

        // Enviar mensaje (evento externo)
        public async Task SendMessageAsync(string to)
        {
            _clock++;
            Console.WriteLine($"{_processName} envía mensaje a {to}. Reloj: {_clock}");
            await PublishEventAsync($"send:{to}:{_clock}");
        }

        // Registra el watcher para el znode raíz
        private async Task WatchForMessagesAsync()
        {
            // El watcher se dispara solo una vez, así que hay que re-registrarlo cada vez
            await _zk.getChildrenAsync(_zkPath, this);
        }

        // Recibir mensajes dirigidos a este proceso (triggered by watcher)
        public async Task ReceiveMessagesAsync(bool rewatch = false)
        {
            var childrenResult = await _zk.getChildrenAsync(_zkPath, this);
            var ordered = childrenResult.Children;
            ordered.Sort(); // Orden total por secuencia ZooKeeper

            var received = new List<string>();
            foreach (var child in ordered)
            {
                if (_processedEvents.Contains(child))
                    continue;

                var data = await _zk.getDataAsync($"{_zkPath}/{child}");
                string eventData = Encoding.UTF8.GetString(data.Data);
                // Mensaje dirigido a este proceso
                if (eventData.StartsWith($"{_processName}:")) { _processedEvents.Add(child); continue; } // Skip own events
                if (eventData.Contains($"send:{_processName}:"))
                {
                    // Extrae el reloj lógico del mensaje recibido
                    var parts = eventData.Split(':');
                    if (parts.Length >= 3 && int.TryParse(parts[3], out int senderClock))
                    {
                        _clock = Math.Max(_clock, senderClock) + 1;
                        Console.WriteLine($"{_processName} recibió mensaje de {parts[0]}. Reloj actualizado: {_clock}");
                        received.Add(eventData);
                    }
                }
                _processedEvents.Add(child);
            }
            if (received.Count == 0)
                Console.WriteLine($"{_processName} no recibió mensajes nuevos.");

            // Re-registrar el watcher si es necesario
            if (rewatch)
                await WatchForMessagesAsync();
        }

        // Publica el evento como un nodo secuencial en ZooKeeper
        private async Task PublishEventAsync(string data)
        {
            string path = $"{_zkPath}/event-";
            await _zk.createAsync(path, Encoding.UTF8.GetBytes($"{_processName}:{data}"), ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL_SEQUENTIAL);
        }

        // Lee y muestra el orden total de eventos
        public async Task ShowTotalOrderAsync()
        {
            var children = await _zk.getChildrenAsync(_zkPath);
            var ordered = children.Children;
            ordered.Sort(); // Orden total por secuencia ZooKeeper

            Console.WriteLine($"\nOrden total de eventos según ZooKeeper:");
            foreach (var child in ordered)
            {
                var data = await _zk.getDataAsync($"{_zkPath}/{child}");
                Console.WriteLine($"{child}: {Encoding.UTF8.GetString(data.Data)}");
            }
        }

        // Ejemplo de uso concurrente con watcher
        public static async Task RunAsync()
        {
            var pA = new LamportWithZooKeeper("A");
            var pB = new LamportWithZooKeeper("B");
            var pC = new LamportWithZooKeeper("C");

            await pA.ConnectAsync();
            await pB.ConnectAsync();
            await pC.ConnectAsync();

            // Cada proceso ejecuta sus eventos en un hilo independiente
            var tA = Task.Run(async () =>
            {
                await pA.InternalEventAsync();
                await Task.Delay(200);
                await pA.SendMessageAsync("C");
            });

            var tB = Task.Run(async () =>
            {
                await pB.InternalEventAsync();
                await Task.Delay(100);
                await pB.SendMessageAsync("C");
            });

            var tC = Task.Run(async () =>
            {
                // C espera un poco para simular concurrencia
                await Task.Delay(300);
                await pC.InternalEventAsync();
                await Task.Delay(100);
                await pC.InternalEventAsync();
            });

            await Task.WhenAll(tA, tB, tC);

            // Espera para asegurar que todos los eventos se publiquen
            await Task.Delay(1000);
            await pA.ShowTotalOrderAsync();
            await _zk?.closeAsync();
        }
    }
}