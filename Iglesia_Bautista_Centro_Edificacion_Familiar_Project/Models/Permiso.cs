using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Permiso
    {
        public Permiso()
        {
            Perfils = new HashSet<Perfil>();
        }

        public int IdPermiso { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Perfil> Perfils { get; set; }
    }
}
