using System;
using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.MVC.Models
{
    public class VueloViewModel
    {
        public int VueloID { get; set; }

        [Display(Name = "Número de Vuelo")]
        public string NumeroVuelo { get; set; }

        public int AerolineaID { get; set; }

        [Display(Name = "Aerolínea")]
        public string NombreAerolinea { get; set; }

        public int AeronaveID { get; set; }

        [Display(Name = "Aeronave")]
        public string ModeloAeronave { get; set; }

        [Display(Name = "Origen")]
        public string Origen { get; set; }

        [Display(Name = "Destino")]
        public string Destino { get; set; }

        [Display(Name = "Fecha de Salida")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaSalida { get; set; }

        [Display(Name = "Fecha de Llegada")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaLlegada { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public int? PuertaID { get; set; }

        [Display(Name = "Puerta")]
        public string NumeroPuerta { get; set; }

        [Display(Name = "Asientos Disponibles")]
        public int AsientosDisponibles { get; set; }

        [Display(Name = "Precio Base")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal PrecioBase { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}