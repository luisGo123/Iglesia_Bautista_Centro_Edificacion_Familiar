using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Almacene
    {
        public Almacene()
        {
            Inventarios = new HashSet<Inventario>();
        }

        public int IdAlmacenes { get; set; }
        public string? Nombre { get; set; }
        public string? Ubicacion { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
