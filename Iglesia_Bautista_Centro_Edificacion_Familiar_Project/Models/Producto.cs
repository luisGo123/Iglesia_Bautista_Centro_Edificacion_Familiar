using System;
using System.Collections.Generic;

namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Entrada = new HashSet<Entrada>();
            Inventarios = new HashSet<Inventario>();
            Salida = new HashSet<Salida>();
        }

        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public string? Color { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdTipoProducto { get; set; }
        public int? IdMarca { get; set; }
        public int? Existencia { get; set; }

        public virtual Categoria? IdCategoriaNavigation { get; set; }
        public virtual Marca? IdMarcaNavigation { get; set; }
        public virtual TipoProducto? IdTipoProductoNavigation { get; set; }
        public virtual ICollection<Entrada> Entrada { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
        public virtual ICollection<Salida> Salida { get; set; }
    }
}
