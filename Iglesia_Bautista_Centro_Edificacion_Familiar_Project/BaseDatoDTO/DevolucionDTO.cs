namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO
{
    public class DevolucionDTO
    {
        public int IdDeVolucion { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public string Observacion { get; set; }
        public int? Cantidad { get; set; }
        public int? IdInventario { get; set; }
        public DateTime? FechaInventario { get; set; }

      
        public int? StockMinimo { get; set; }
        public int? StockMaximo { get; set; }

      

    }
}
