using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class TipoProducto
    {
        public TipoProducto()
        {
            Productos = new HashSet<Producto>();
        }

        public int IdTipoProducto { get; set; }
        public string? Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
