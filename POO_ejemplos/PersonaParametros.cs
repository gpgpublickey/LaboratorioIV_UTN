using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_ejemplos
{
    internal class PersonaParametros
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public decimal Altura { get; set; }

        public PersonaParametros()
        {
            //throw new ArgumentException("Debe especificar los parámetros");
        }
    }
}
