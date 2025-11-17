using System;
using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.MVC.Models
{
    public class CrearVueloViewModel
    {
        [Required(ErrorMessage = "El número de vuelo es requerido")]
        [Display(Name = "Número de Vuelo")]
        public string NumeroVuelo { get; set; }

        [Required(ErrorMessage = "Seleccione una aerolínea")]
        [Display(Name = "Aerolínea")]
        public int AerolineaID { get; set; }

        [Required(ErrorMessage = "Seleccione una aeronave")]
        [Display(Name = "Aeronave")]
        public int AeronaveID { get; set; }

        [Required(ErrorMessage = "El origen es requerido")]
        [Display(Name = "Origen")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El destino es requerido")]
        [Display(Name = "Destino")]
        public string Destino { get; set; }

        [Required(ErrorMessage = "La fecha de salida es requerida")]
        [Display(Name = "Fecha y Hora de Salida")]
        [DataType(DataType.DateTime)]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "La fecha de llegada es requerida")]
        [Display(Name = "Fecha y Hora de Llegada")]
        [DataType(DataType.DateTime)]
        public DateTime FechaLlegada { get; set; }

        [Required(ErrorMessage = "El precio base es requerido")]
        [Display(Name = "Precio Base (COP)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal PrecioBase { get; set; }
    }
}