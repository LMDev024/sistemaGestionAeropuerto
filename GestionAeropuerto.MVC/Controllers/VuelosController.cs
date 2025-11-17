using GestionAeropuerto.MVC.Models;
using GestionAeropuerto.MVC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionAeropuerto.MVC.Controllers
{
    public class VuelosController : Controller
    {
        private readonly ApiService _apiService;

        public VuelosController()
        {
            _apiService = new ApiService();
        }

        // GET: Vuelos
        public async Task<ActionResult> Index()
        {
            try
            {
                var vuelos = await _apiService.GetAsync<List<VueloViewModel>>("vuelos");
                return View(vuelos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar vuelos: {ex.Message}";
                return View(new List<VueloViewModel>());
            }
        }

        // GET: Vuelos/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var vuelo = await _apiService.GetAsync<VueloViewModel>($"vuelos/{id}");

                if (vuelo == null)
                {
                    return HttpNotFound();
                }

                return View(vuelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar el vuelo: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Vuelos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vuelos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CrearVueloViewModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _apiService.PostAsync<VueloViewModel>("vuelos", modelo);

                    if (resultado != null)
                    {
                        TempData["Success"] = "Vuelo creado exitosamente";
                        return RedirectToAction("Index");
                    }
                }

                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al crear el vuelo: {ex.Message}";
                return View(modelo);
            }
        }

        // GET: Vuelos/TableroSalidas
        public async Task<ActionResult> TableroSalidas()
        {
            try
            {
                var fecha = DateTime.Today;
                var vuelos = await _apiService.GetAsync<List<VueloViewModel>>($"vuelos/fecha/{fecha:yyyy-MM-dd}");
                return View(vuelos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar tablero: {ex.Message}";
                return View(new List<VueloViewModel>());
            }
        }

        // POST: Vuelos/ActualizarEstado/5
        [HttpPost]
        public async Task<ActionResult> ActualizarEstado(int id, string nuevoEstado)
        {
            try
            {
                var data = new { NuevoEstado = nuevoEstado, Observaciones = "Actualizado desde MVC" };
                var resultado = await _apiService.PatchAsync($"vuelos/{id}/estado", data);

                if (resultado)
                {
                    TempData["Success"] = "Estado actualizado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar el estado";
                }

                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}