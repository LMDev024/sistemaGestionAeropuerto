using GestionAeropuerto.MVC.Models;
using GestionAeropuerto.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionAeropuerto.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController()
        {
            _apiService = new ApiService();
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                // Obtener todos los vuelos
                var vuelos = await _apiService.GetAsync<List<VueloViewModel>>("vuelos");

                // Obtener todas las reservas
                var reservas = await _apiService.GetAsync<List<ReservaViewModel>>("reservas");

                // Obtener todos los pasajeros
                var pasajeros = await _apiService.GetAsync<List<PasajeroViewModel>>("pasajeros");

                // Calcular estadísticas
                ViewBag.TotalVuelos = vuelos.Count;
                ViewBag.VuelosHoy = vuelos.Count(v => v.FechaSalida.Date == DateTime.Today);
                ViewBag.TotalReservas = reservas.Count;
                ViewBag.ReservasActivas = reservas.Count(r => r.Estado != "Cancelada");
                ViewBag.TotalPasajeros = pasajeros.Count;

                // Vuelos por estado
                ViewBag.VuelosProgramados = vuelos.Count(v => v.Estado == "Programado");
                ViewBag.VuelosAbordando = vuelos.Count(v => v.Estado == "Abordando");
                ViewBag.VuelosEnVuelo = vuelos.Count(v => v.Estado == "En Vuelo");
                ViewBag.VuelosAterrizados = vuelos.Count(v => v.Estado == "Aterrizado");

                // Próximos vuelos (siguiente 5)
                var proximosVuelos = vuelos
                    .Where(v => v.FechaSalida >= DateTime.Now && v.Estado == "Programado")
                    .OrderBy(v => v.FechaSalida)
                    .Take(5)
                    .ToList();
                ViewBag.ProximosVuelos = proximosVuelos;

                // Ingresos estimados
                ViewBag.IngresosEstimados = reservas
                    .Where(r => r.Estado != "Cancelada")
                    .Sum(r => r.Precio);

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar el dashboard: {ex.Message}";
                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sistema de Gestión Aeroportuaria";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Información de contacto";
            return View();
        }
    }
}