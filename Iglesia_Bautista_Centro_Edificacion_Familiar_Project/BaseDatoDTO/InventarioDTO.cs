namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO
{
    public class InventarioDTO
    {
        public int IdInventario { get; set; }
        public DateTime? FechaInventario { get; set; }
        public string NombreProducto { get; set; } 
        public string ColorProducto { get; set; } 
        public int? IdCategoria { get; set; } 
        public string NombreCategoria { get; set; } 
        public int? IdMarca { get; set; } 
        public string NombreMarca { get; set; } 
        public int? StockMinimo { get; set; }
        public int? StockMaximo { get; set; }
        public int? Cantidad { get; set; }
        public bool? Statud { get; set; }
        public string NombreAlmacen { get; set; }
    }
}
