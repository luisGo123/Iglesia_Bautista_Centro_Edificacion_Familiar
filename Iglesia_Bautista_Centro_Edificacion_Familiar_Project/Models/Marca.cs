using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Marca
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdMarca { get; set; }
        public string? Nombre { get; set; }
        public int? IdModeloMarca { get; set; }

        public virtual ModeloMarca? IdModeloMarcaNavigation { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
