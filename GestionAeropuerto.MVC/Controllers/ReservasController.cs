using GestionAeropuerto.MVC.Models;
using GestionAeropuerto.MVC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionAeropuerto.MVC.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApiService _apiService;

        public ReservasController()
        {
            _apiService = new ApiService();
        }

        // GET: Reservas
        public async Task<ActionResult> Index()
        {
            try
            {
                var reservas = await _apiService.GetAsync<List<ReservaViewModel>>("reservas");
                return View(reservas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar reservas: {ex.Message}";
                return View(new List<ReservaViewModel>());
            }
        }

        // GET: Reservas/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var reserva = await _apiService.GetAsync<ReservaViewModel>($"reservas/{id}");

                if (reserva == null)
                {
                    return HttpNotFound();
                }

                return View(reserva);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar la reserva: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Reservas/BuscarPorCodigo
        public ActionResult BuscarPorCodigo()
        {
            return View();
        }

        // POST: Reservas/BuscarPorCodigo
        [HttpPost]
        public async Task<ActionResult> BuscarPorCodigo(string codigoReserva)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigoReserva))
                {
                    ViewBag.Error = "Ingrese un código de reserva";
                    return View();
                }

                var reserva = await _apiService.GetAsync<ReservaViewModel>($"reservas/codigo/{codigoReserva}");

                if (reserva == null)
                {
                    ViewBag.Error = "Reserva no encontrada";
                    return View();
                }

                return RedirectToAction("Details", new { id = reserva.ReservaID });
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al buscar la reserva: {ex.Message}";
                return View();
            }
        }

        // GET: Reservas/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                // Cargar vuelos disponibles
                var vuelos = await _apiService.GetAsync<List<VueloViewModel>>("vuelos");
                ViewBag.Vuelos = new SelectList(vuelos, "VueloID", "NumeroVuelo");

                // Cargar pasajeros
                var pasajeros = await _apiService.GetAsync<List<PasajeroViewModel>>("pasajeros");
                ViewBag.Pasajeros = new SelectList(pasajeros, "PasajeroID", "Nombre");

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar datos: {ex.Message}";
                return View();
            }
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReservaViewModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new
                    {
                        VueloID = modelo.VueloID,
                        PasajeroID = modelo.PasajeroID,
                        NumeroAsiento = modelo.NumeroAsiento,
                        Clase = modelo.Clase,
                        Equipaje = modelo.Equipaje
                    };

                    var resultado = await _apiService.PostAsync<ReservaViewModel>("reservas", data);

                    if (resultado != null)
                    {
                        TempData["Success"] = "Reserva creada exitosamente. Código: " + resultado.CodigoReserva;
                        return RedirectToAction("Details", new { id = resultado.ReservaID });
                    }
                }

                // Recargar listas
                var vuelos = await _apiService.GetAsync<List<VueloViewModel>>("vuelos");
                ViewBag.Vuelos = new SelectList(vuelos, "VueloID", "NumeroVuelo");

                var pasajeros = await _apiService.GetAsync<List<PasajeroViewModel>>("pasajeros");
                ViewBag.Pasajeros = new SelectList(pasajeros, "PasajeroID", "Nombre");

                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al crear la reserva: {ex.Message}";
                return View(modelo);
            }
        }

        // POST: Reservas/Cancelar/5
        [HttpPost]
        public async Task<ActionResult> Cancelar(int id, string motivo)
        {
            try
            {
                var url = $"reservas/{id}";
                if (!string.IsNullOrWhiteSpace(motivo))
                {
                    url += $"?motivo={Uri.EscapeDataString(motivo)}";
                }

                var resultado = await _apiService.DeleteAsync(url);

                if (resultado)
                {
                    TempData["Success"] = "Reserva cancelada exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo cancelar la reserva";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}