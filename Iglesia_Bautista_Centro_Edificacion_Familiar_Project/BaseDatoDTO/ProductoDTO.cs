namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Color { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; } // Para almacenar el nombre de la categoría
        public int IdTipoProducto { get; set; }
        public string TipoProductoNombre { get; set; } // Para almacenar el nombre del tipo de producto
        public int IdMarca { get; set; }
        public string MarcaNombre { get; set; } // Para almacenar el nombre de la marca
        public int Existencia { get; set; }
    }
}
