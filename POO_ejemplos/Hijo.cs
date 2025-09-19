using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_ejemplos
{
    internal class Hijo : Persona, IAmigable, ISaludable
    {
        public Hijo() : base()
        {
            
        }

        public Hijo(int edad) : base("Hijo")
        {
            Edad = edad;
        }

        public static Persona Crear(PersonaParametros parametros)
        {
            return new Hijo(parametros.Edad);
        }

        public override void DecirHola()
        {
            throw new NotImplementedException();
        }

        public void Despedirse()
        {
            throw new NotImplementedException();
        }

        public void Ducharse()
        {
            throw new NotImplementedException();
        }

        public override void Saludar()
        {
            base.Saludar();
        }
    }
}
