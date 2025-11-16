namespace GestionAeropuerto.API.Models
{
    public class Reserva
    {
        public int ReservaID { get; set; }
        public string CodigoReserva { get; set; } = string.Empty;
        public int VueloID { get; set; }
        public int PasajeroID { get; set; }
        public string NumeroAsiento { get; set; } = string.Empty;
        public string Clase { get; set; } = "Economica";
        public string Estado { get; set; } = "Confirmada";
        public decimal Precio { get; set; }
        public int Equipaje { get; set; } = 1;
        public DateTime FechaReserva { get; set; }

        public string? NombrePasajero { get; set; }
        public string? NumeroVuelo { get; set; }
   
    }
}
