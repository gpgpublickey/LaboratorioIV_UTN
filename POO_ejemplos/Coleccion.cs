using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace POO_ejemplos
{
    internal class Coleccion<T> : IAgregable<T>
    {
        private T[] Lista { get; set; }

        public Coleccion(int cantidad)
        {
            Lista = new T[cantidad];
        }

        public void Agregar(int indice, T elemento)
        {
            if (indice >= 0 && indice < Lista.Length)
            {
                Lista[indice] = elemento;
            }
            else
            {
                throw new IndexOutOfRangeException("Índice fuera de rango");
            }
        }

        public void Agregar(T elemento)
        {
            Lista = Lista.Append(elemento).ToArray();
        }

        public void Serializar()
        {
            foreach (var item in Lista)
            {
                Console.WriteLine(JsonSerializer.Serialize(item));
            }
        }

        public override string ToString()
        {
            return string.Join(", ", Lista);
        }
    }
}
