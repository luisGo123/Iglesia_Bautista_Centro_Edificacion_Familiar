using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class AprovadoEntrega
    {
        public int IdAprovadoEntrega { get; set; }
        public int? IdSolicitante { get; set; }
        public int? IdSolicitudPrestamo { get; set; }
        public DateTime? FechaAprovacion { get; set; }
        public int? Cantidad { get; set; }
        public string? Observacion { get; set; }

        public virtual Solicitante? IdSolicitanteNavigation { get; set; }
        public virtual SolicitudPrestamo? IdSolicitudPrestamoNavigation { get; set; }
    }
}
