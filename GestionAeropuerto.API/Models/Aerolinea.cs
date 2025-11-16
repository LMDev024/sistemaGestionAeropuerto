namespace GestionAeropuerto.API.Models
{
    public class Aerolinea
    {
        public int AerolineaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoIATA { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
    }
}
