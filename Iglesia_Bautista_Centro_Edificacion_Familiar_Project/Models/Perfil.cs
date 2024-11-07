using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdPerfil { get; set; }
        public string? Nombre { get; set; }
        public int? IdPermiso { get; set; }

        public virtual Permiso? IdPermisoNavigation { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
