namespace Iglesia_Bautista_Centro_Edificacion_Familiar_Project.BaseDatoDTO
{
    public class AprovadoEntreagaDTO
    {
        public int IdAprovadoEntrega { get; set; }

        public int IdSolicitante { get; set; }

        public int IdSolicituPrestamo { get; set; }
        public string NombreSolicitante { get; set; }
        public string NombrePrestamo { get; set; }
        public DateTime FechaAprovacion { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
    }
}
