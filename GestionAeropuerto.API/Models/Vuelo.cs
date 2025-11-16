namespace GestionAeropuerto.API.Models
{
    public class Vuelo
    {
        public int VueloID { get; set; }
        public string NumeroVuelo { get; set; } = string.Empty;
        public int AerolineaID { get; set; }
        public int AeronaveID { get; set; }
        public string Origen { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public DateTime FechaSalida { get; set; }
        public DateTime FechaLlegada { get; set; }
        public string Estado { get; set; } = "Programado";
        public int? PuertaID { get; set; }
        public int AsientosDisponibles { get; set; }
        public decimal PrecioBase   { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string? NombreAerolinea { get; set; }
        public string? ModeloAeronave { get; set; } 
        public string? NumeroPuerta { get; set; }   

    }
}
