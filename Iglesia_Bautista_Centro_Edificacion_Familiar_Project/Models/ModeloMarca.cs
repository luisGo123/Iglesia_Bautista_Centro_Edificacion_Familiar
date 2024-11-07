using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class ModeloMarca
    {
        public ModeloMarca()
        {
            Marcas = new HashSet<Marca>();
        }

        public int IdModeloMarca { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Marca> Marcas { get; set; }
    }
}
