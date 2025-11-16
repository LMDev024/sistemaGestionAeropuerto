namespace GestionAeropuerto.API.Models
{
    public class Puerta
    {
        public int PuertaID { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Terminal { get; set; } = string.Empty;
        public string Estado { get; set; } = "Disponible";
    }
}
