using GestionAeropuerto.MVC.Models;
using GestionAeropuerto.MVC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionAeropuerto.MVC.Controllers
{
    public class PasajerosController : Controller
    {
        private readonly ApiService _apiService;

        public PasajerosController()
        {
            _apiService = new ApiService();
        }

        // GET: Pasajeros
        public async Task<ActionResult> Index()
        {
            try
            {
                var pasajeros = await _apiService.GetAsync<List<PasajeroViewModel>>("pasajeros");
                return View(pasajeros);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar pasajeros: {ex.Message}";
                return View(new List<PasajeroViewModel>());
            }
        }

        // GET: Pasajeros/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var pasajero = await _apiService.GetAsync<PasajeroViewModel>($"pasajeros/{id}");

                if (pasajero == null)
                {
                    return HttpNotFound();
                }

                // Obtener reservas del pasajero
                var reservas = await _apiService.GetAsync<List<ReservaViewModel>>($"reservas/pasajero/{id}");
                ViewBag.Reservas = reservas;

                return View(pasajero);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar el pasajero: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Pasajeros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pasajeros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PasajeroViewModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _apiService.PostAsync<PasajeroViewModel>("pasajeros", modelo);

                    if (resultado != null)
                    {
                        TempData["Success"] = "Pasajero registrado exitosamente";
                        return RedirectToAction("Details", new { id = resultado.PasajeroID });
                    }
                }

                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al crear el pasajero: {ex.Message}";
                return View(modelo);
            }
        }

        // GET: Pasajeros/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var pasajero = await _apiService.GetAsync<PasajeroViewModel>($"pasajeros/{id}");

                if (pasajero == null)
                {
                    return HttpNotFound();
                }

                return View(pasajero);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al cargar el pasajero: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Pasajeros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PasajeroViewModel modelo)
        {
            try
            {
                if (id != modelo.PasajeroID)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }

                if (ModelState.IsValid)
                {
                    var resultado = await _apiService.PutAsync($"pasajeros/{id}", modelo);

                    if (resultado)
                    {
                        TempData["Success"] = "Pasajero actualizado exitosamente";
                        return RedirectToAction("Details", new { id = id });
                    }
                    else
                    {
                        ViewBag.Error = "No se pudo actualizar el pasajero";
                    }
                }

                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al actualizar el pasajero: {ex.Message}";
                return View(modelo);
            }
        }
    }
}