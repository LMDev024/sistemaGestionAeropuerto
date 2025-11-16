using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.API.DTOs
{
    public class ActualizarEstadoVueloDto
    {
        [Required]
        public string NuevoEstado { get; set; } = string.Empty;

        public string? Observaciones { get; set; }
    }
}