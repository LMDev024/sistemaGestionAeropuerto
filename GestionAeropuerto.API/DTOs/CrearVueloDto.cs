
using System.ComponentModel.DataAnnotations;  

namespace GestionAeropuerto.API.DTOs
{
    public class CrearVueloDto
    {
        [Required(ErrorMessage = "El número de vuelo es requerido")]
        public string NumeroVuelo { get; set; } = string.Empty;
        [Required]
        public int AerolineaID { get; set; }
        [Required]
        public int AeronaveID { get; set; }
        [Required]
        public string Origen { get; set; } = string.Empty;
        [Required]
        public string Destino { get; set; } = string.Empty;
        [Required]
        public DateTime FechaSalida { get; set; }
        [Required]
        public DateTime FechaLlegada { get; set; }
        [Required]
        [Range(0.01,double.MaxValue, ErrorMessage ="El precio debe ser mayor a 0")]
        public decimal PrecioBase { get; set; }

    }
}
