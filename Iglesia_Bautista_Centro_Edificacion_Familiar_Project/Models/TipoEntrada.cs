using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class TipoEntrada
    {
        public TipoEntrada()
        {
            Inventarios = new HashSet<Inventario>();
        }

        public int IdTipoEntrada { get; set; }
        public string? Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
