using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Solicitante
    {
        public Solicitante()
        {
            AprovadoEntregas = new HashSet<AprovadoEntrega>();
        }

        public int IdSolicitante { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Cedula { get; set; }
        public string? Dirrecion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        [JsonIgnore]
        public virtual ICollection<AprovadoEntrega> AprovadoEntregas { get; set; }
    }
}
