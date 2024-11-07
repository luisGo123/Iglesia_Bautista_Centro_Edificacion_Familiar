using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class DeVolucion
    {
        public int IdDeVolucion { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int? IdInventario { get; set; }
        public string? Observacion { get; set; }
        public int? Cantidad { get; set; }

        public virtual Inventario? IdInventarioNavigation { get; set; }
    }
}
