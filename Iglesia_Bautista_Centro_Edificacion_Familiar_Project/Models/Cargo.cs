using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Cargo
    {
        public Cargo()
        {
            Responsables = new HashSet<Responsable>();
        }

        public int IdCargo { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Responsable> Responsables { get; set; }
    }
}
