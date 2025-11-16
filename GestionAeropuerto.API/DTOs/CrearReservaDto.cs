using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.API.DTOs
{
    public class CrearReservaDto
    {
        [Required]
        public int VueloID { get; set; }

        [Required]
        public int PasajeroID { get; set; }

        [Required]
        public string NumeroAsiento { get; set; } = string.Empty;

        [Required]
        public string Clase { get; set; } = "Economica";

        [Range(1, 5)]
        public int Equipaje { get; set; } = 1;
    }
}