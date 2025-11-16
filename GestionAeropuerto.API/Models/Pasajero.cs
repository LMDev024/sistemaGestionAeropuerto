namespace GestionAeropuerto.API.Models
{
    public class Pasajero
    {
        public int PasajeroID { get; set; }
        public string TipoDocumento { get; set; } = "CC";
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
