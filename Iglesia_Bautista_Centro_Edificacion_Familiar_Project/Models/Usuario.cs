using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Almacenes = new HashSet<Almacene>();
        }

        public int IdUsuario { get; set; }
        public string? NombreCompleto { get; set; }
        public string? ApellidoCompleto { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Telefono { get; set; }
        public string? Cedula { get; set; }
        public int? IdPerfil { get; set; }


        [JsonIgnore]
        public virtual Perfil? IdPerfilNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Almacene> Almacenes { get; set; }
    }
}
