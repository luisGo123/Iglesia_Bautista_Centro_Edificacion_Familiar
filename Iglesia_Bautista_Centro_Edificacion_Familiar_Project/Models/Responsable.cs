using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Responsable
    {
        public Responsable()
        {
            SolicitudPrestamos = new HashSet<SolicitudPrestamo>();
        }

        public int IdResponsable { get; set; }
        public string? Nombre { get; set; }
        public string? Cedula { get; set; }
        public int? IdCargo { get; set; }

        public virtual Cargo? IdCargoNavigation { get; set; }
        public virtual ICollection<SolicitudPrestamo> SolicitudPrestamos { get; set; }
    }
}
