using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Entrada
    {
        public int IdEntrada { get; set; }
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }

        public virtual Producto? IdProductoNavigation { get; set; }
    }
}
