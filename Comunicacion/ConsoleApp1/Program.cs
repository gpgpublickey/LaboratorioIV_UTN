using Grpc.Net.Client;
using GrpcService2;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var cliente = new MiServicio.ServiceClient();
            var tarea = cliente.GetDataAsync(1234);
            Task.WaitAll(tarea);
            Console.WriteLine(tarea.Result);
            using var channel = GrpcChannel.ForAddress("https://localhost:7128");
            var client = new GrpcService2.Greeter.GreeterClient(channel);
            var response = client.SayHelloAsync(new HelloRequest { Name = "Juan" });
  
            Console.WriteLine(await response);
        }
    }
}
