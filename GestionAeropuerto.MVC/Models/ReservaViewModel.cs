using System;
using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.MVC.Models
{
    public class ReservaViewModel
    {
        public int ReservaID { get; set; }

        [Display(Name = "Código de Reserva")]
        public string CodigoReserva { get; set; }

        public int VueloID { get; set; }

        [Display(Name = "Número de Vuelo")]
        public string NumeroVuelo { get; set; }

        public int PasajeroID { get; set; }

        [Display(Name = "Pasajero")]
        public string NombrePasajero { get; set; }

        [Display(Name = "Número de Asiento")]
        public string NumeroAsiento { get; set; }

        [Display(Name = "Clase")]
        public string Clase { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Precio { get; set; }

        [Display(Name = "Equipaje")]
        public int Equipaje { get; set; }

        [Display(Name = "Fecha de Reserva")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime FechaReserva { get; set; }
    }
}