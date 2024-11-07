using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class SolicitudPrestamo
    {
        public SolicitudPrestamo()
        {
            AprovadoEntregas = new HashSet<AprovadoEntrega>();
        }

        public int IdSolicitudPrestamo { get; set; }
        public DateTime? FechaOperaciones { get; set; }
        public int? IdInventario { get; set; }
        public int? Cantidad { get; set; }
        public string? Lugar { get; set; }
        public string? FechaEntrega { get; set; }
        public int? IdResponsable { get; set; }

      

        [JsonIgnore]
        public virtual Inventario? IdInventarioNavigation { get; set; }
        [JsonIgnore]
        public virtual Responsable? IdResponsableNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<AprovadoEntrega> AprovadoEntregas { get; set; }
      
    }
}
