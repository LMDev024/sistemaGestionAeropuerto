using GestionAeropuerto.API.Models;
using GestionAeropuerto.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GestionAeropuerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasajerosController : ControllerBase
    {
        private readonly IPasajeroRepository _pasajeroRepository;

        public PasajerosController(IPasajeroRepository pasajeroRepository)
        {
            _pasajeroRepository = pasajeroRepository;
        }

        // GET: api/pasajeros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pasajero>>> GetAll()
        {
            try
            {
                var pasajeros = await _pasajeroRepository.GetAllAsync();
                return Ok(pasajeros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener pasajeros", error = ex.Message });
            }
        }

        // GET: api/pasajeros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pasajero>> GetById(int id)
        {
            try
            {
                var pasajero = await _pasajeroRepository.GetByIdAsync(id);

                if (pasajero == null)
                    return NotFound(new { message = $"Pasajero con ID {id} no encontrado" });

                return Ok(pasajero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el pasajero", error = ex.Message });
            }
        }

        // GET: api/pasajeros/documento/CC/1234567890
        [HttpGet("documento/{tipoDocumento}/{numeroDocumento}")]
        public async Task<ActionResult<Pasajero>> GetByDocumento(string tipoDocumento, string numeroDocumento)
        {
            try
            {
                var pasajero = await _pasajeroRepository.GetByDocumentoAsync(tipoDocumento, numeroDocumento);

                if (pasajero == null)
                    return NotFound(new { message = "Pasajero no encontrado" });

                return Ok(pasajero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el pasajero", error = ex.Message });
            }
        }

        // POST: api/pasajeros
        [HttpPost]
        public async Task<ActionResult<Pasajero>> Create([FromBody] Pasajero pasajero)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Verificar si ya existe
                var existente = await _pasajeroRepository.GetByDocumentoAsync(
                    pasajero.TipoDocumento,
                    pasajero.NumeroDocumento
                );

                if (existente != null)
                    return Conflict(new { message = "Ya existe un pasajero con ese documento" });

                var pasajeroId = await _pasajeroRepository.CreateAsync(pasajero);
                pasajero.PasajeroID = pasajeroId;

                return CreatedAtAction(nameof(GetById), new { id = pasajeroId }, pasajero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el pasajero", error = ex.Message });
            }
        }

        // PUT: api/pasajeros/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Pasajero pasajero)
        {
            try
            {
                if (id != pasajero.PasajeroID)
                    return BadRequest(new { message = "El ID no coincide" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _pasajeroRepository.UpdateAsync(pasajero);

                if (!resultado)
                    return NotFound(new { message = "Pasajero no encontrado" });

                return Ok(new { message = "Pasajero actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el pasajero", error = ex.Message });
            }
        }
    }
}