using System;
using System.ComponentModel.DataAnnotations;

namespace GestionAeropuerto.MVC.Models
{
    public class PasajeroViewModel
    {
        public int PasajeroID { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "El tipo de documento es requerido")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Número de Documento")]
        [Required(ErrorMessage = "El número de documento es requerido")]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "La nacionalidad es requerida")]
        public string Nacionalidad { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
    }
}