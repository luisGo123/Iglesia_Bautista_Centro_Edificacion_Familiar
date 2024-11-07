namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO
{
    public class EntradaDTO
    {
        public int IdEntrada { get; set; }
        public int? IdProducto { get; set; }
        public int? Cantidad { get; set; }
        public string NombreProducto { get; set; }
        public string Color { get; set; } 
        public int? Existencia { get; set; }   
        public string Categoria { get; set; }
        public string TipoProducto { get; set; } 
        public string Marca { get; set; } 
    }
}
