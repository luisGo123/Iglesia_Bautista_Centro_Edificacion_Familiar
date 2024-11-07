using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdCategoria { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
