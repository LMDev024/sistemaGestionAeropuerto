using GestionAeropuerto.API.DTOs;
using GestionAeropuerto.API.Models;
using GestionAeropuerto.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GestionAeropuerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VuelosController : ControllerBase
    {
        private readonly IVueloRepository _vueloRepository;

        public VuelosController(IVueloRepository vueloRepository)
        {
            _vueloRepository = vueloRepository;
        }

        // GET: api/vuelos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vuelo>>> GetAll()
        {
            try
            {
                var vuelos = await _vueloRepository.GetAllAsync();
                return Ok(vuelos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener vuelos", error = ex.Message });
            }
        }

        // GET: api/vuelos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vuelo>> GetById(int id)
        {
            try
            {
                var vuelo = await _vueloRepository.GetByIdAsync(id);

                if (vuelo == null)
                    return NotFound(new { message = $"Vuelo con ID {id} no encontrado" });

                return Ok(vuelo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el vuelo", error = ex.Message });
            }
        }

        // GET: api/vuelos/fecha/2024-11-15
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<Vuelo>>> GetByFecha(DateTime fecha)
        {
            try
            {
                var vuelos = await _vueloRepository.GetByFechaAsync(fecha);
                return Ok(vuelos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener vuelos", error = ex.Message });
            }
        }

        // POST: api/vuelos
        [HttpPost]
        public async Task<ActionResult<Vuelo>> Create([FromBody] CrearVueloDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var vuelo = new Vuelo
                {
                    NumeroVuelo = dto.NumeroVuelo,
                    AerolineaID = dto.AerolineaID,
                    AeronaveID = dto.AeronaveID,
                    Origen = dto.Origen,
                    Destino = dto.Destino,
                    FechaSalida = dto.FechaSalida,
                    FechaLlegada = dto.FechaLlegada,
                    PrecioBase = dto.PrecioBase
                };

                var vueloId = await _vueloRepository.CreateAsync(vuelo);
                vuelo.VueloID = vueloId;

                return CreatedAtAction(nameof(GetById), new { id = vueloId }, vuelo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el vuelo", error = ex.Message });
            }
        }

        // PATCH: api/vuelos/5/estado
        [HttpPatch("{id}/estado")]
        public async Task<ActionResult> UpdateEstado(int id, [FromBody] ActualizarEstadoVueloDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _vueloRepository.UpdateEstadoAsync(id, dto.NuevoEstado, dto.Observaciones);

                if (!resultado)
                    return NotFound(new { message = "No se pudo actualizar el estado del vuelo" });

                return Ok(new { message = "Estado actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el estado", error = ex.Message });
            }
        }

        // POST: api/vuelos/5/puerta/3
        [HttpPost("{vueloId}/puerta/{puertaId}")]
        public async Task<ActionResult> AsignarPuerta(int vueloId, int puertaId)
        {
            try
            {
                var resultado = await _vueloRepository.AsignarPuertaAsync(vueloId, puertaId);

                if (!resultado)
                    return BadRequest(new { message = "No se pudo asignar la puerta" });

                return Ok(new { message = "Puerta asignada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al asignar la puerta", error = ex.Message });
            }
        }
    }
}