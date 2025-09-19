using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_ejemplos
{
    public class AgregarProductoRequestDTO
    {
        public int? Id { get; set; }

        public string Nombre { get; set; }

        public int NroPieza { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        internal void HacerDescuento(int porcentaje)
        {

        }
    }
}
