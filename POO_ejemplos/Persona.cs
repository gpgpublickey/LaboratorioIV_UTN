using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_ejemplos
{
    internal abstract class Persona
    {
        public string Nombre { get; set; }

        public int Edad { get; set; }

        public decimal Altura { get; set; }


        protected Persona()
        {
            Nombre = "NN";
            Edad = 0;
            Altura = 0;
        }

        protected Persona(string nombre)
        {
            Nombre = nombre;
        }

        private Persona(string nombre, int edad, decimal altura) : this()
        {
            Nombre = nombre;
            Edad = edad;
            Altura = altura;
        }

        public abstract void DecirHola();

        public virtual void Saludar()
        {
            Console.WriteLine($"Hola, soy {Nombre}");
        }
    }
}
