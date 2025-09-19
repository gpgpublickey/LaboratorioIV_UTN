using System.Runtime.CompilerServices;

namespace POO_ejemplos
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var edad = 88;
            var nombre = "Cristian";
            nombre.Capitalizar();
            var nuevaEdad = edad.IncrementarEn(1);
            var producto = new AgregarProductoRequestDTO
            {
                Id = null,
                Nombre = "Producto 1",
                NroPieza = 1234,
                Cantidad = 10,
                Precio = 99.99m
            };

            producto.Precio = 0;
            producto.HacerDescuento(100);
            Action<int, string> HelloWorld = (x, y) => Console.WriteLine($"{x}{y}");
            HelloWorld(1, "World");
            Console.WriteLine(string.Format("TESTING {0}{1}!, {2}", "HELLO", "WORLD","BYE!",4,5,6));
            Console.WriteLine($"TESTING {"HELLO"}{"WORLD"}!, {"BYE!"}");
            Console.WriteLine("TESTING " + "HELLO" + "WORLD" + "!, " + "BYE!");
            var coleccion = new Coleccion<int>(10);
            coleccion.Agregar(8, 1);
            coleccion.Agregar(1);
            Console.WriteLine(coleccion.ToString());
            var coleccionDeObjetos = new Coleccion<Persona>(5);

            //var persona = new Persona("Cristian", 88, 1.75m);
            var persona2 = new Hijo(11);
            persona2.Saludar();

            coleccionDeObjetos.Agregar(new Empleado());
            coleccionDeObjetos.Agregar(new Hijo());
            coleccionDeObjetos.Serializar();
            Console.ReadLine();
        }

        public static string Capitalizar(this string texto)
        {
            return texto.ToUpper();
        }

        public static int IncrementarEn(this int numero, int cantidad)
        {
            return numero + cantidad;
        }
    }
}
