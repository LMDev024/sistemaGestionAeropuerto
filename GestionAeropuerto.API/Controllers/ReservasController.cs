using GestionAeropuerto.API.DTOs;
using GestionAeropuerto.API.Models;
using GestionAeropuerto.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GestionAeropuerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservasController(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        // GET: api/reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetAll()
        {
            try
            {
                var reservas = await _reservaRepository.GetAllAsync();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener reservas", error = ex.Message });
            }
        }

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetById(int id)
        {
            try
            {
                var reserva = await _reservaRepository.GetByIdAsync(id);

                if (reserva == null)
                    return NotFound(new { message = $"Reserva con ID {id} no encontrada" });

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la reserva", error = ex.Message });
            }
        }

        // GET: api/reservas/codigo/ABC123
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Reserva>> GetByCodigo(string codigo)
        {
            try
            {
                var reserva = await _reservaRepository.GetByCodigoAsync(codigo);

                if (reserva == null)
                    return NotFound(new { message = $"Reserva con código {codigo} no encontrada" });

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la reserva", error = ex.Message });
            }
        }

        // GET: api/reservas/vuelo/5
        [HttpGet("vuelo/{vueloId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetByVuelo(int vueloId)
        {
            try
            {
                var reservas = await _reservaRepository.GetByVueloAsync(vueloId);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener reservas del vuelo", error = ex.Message });
            }
        }

        // GET: api/reservas/pasajero/5
        [HttpGet("pasajero/{pasajeroId}")]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetByPasajero(int pasajeroId)
        {
            try
            {
                var reservas = await _reservaRepository.GetByPasajeroAsync(pasajeroId);
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener reservas del pasajero", error = ex.Message });
            }
        }

        // POST: api/reservas
        [HttpPost]
        public async Task<ActionResult<Reserva>> Create([FromBody] CrearReservaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var reserva = new Reserva
                {
                    VueloID = dto.VueloID,
                    PasajeroID = dto.PasajeroID,
                    NumeroAsiento = dto.NumeroAsiento,
                    Clase = dto.Clase,
                    Equipaje = dto.Equipaje
                };

                var reservaId = await _reservaRepository.CreateAsync(reserva);
                reserva.ReservaID = reservaId;

                return CreatedAtAction(nameof(GetById), new { id = reservaId }, reserva);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la reserva", error = ex.Message });
            }
        }

        // DELETE: api/reservas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Cancel(int id, [FromQuery] string? motivo = null)
        {
            try
            {
                var resultado = await _reservaRepository.CancelarAsync(id, motivo);

                if (!resultado)
                    return NotFound(new { message = "No se pudo cancelar la reserva" });

                return Ok(new { message = "Reserva cancelada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al cancelar la reserva", error = ex.Message });
            }
        }
    }
}