using GestionAeropuerto.API.Models;
using GestionAeropuerto.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GestionAeropuerto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuertasController : ControllerBase
    {
        private readonly IPuertaRepository _puertaRepository;

        public PuertasController(IPuertaRepository puertaRepository)
        {
            _puertaRepository = puertaRepository;
        }

        // GET: api/puertas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puerta>>> GetAll()
        {
            try
            {
                var puertas = await _puertaRepository.GetAllAsync();
                return Ok(puertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener puertas", error = ex.Message });
            }
        }

        // GET: api/puertas/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<Puerta>>> GetDisponibles()
        {
            try
            {
                var puertas = await _puertaRepository.GetDisponiblesAsync();
                return Ok(puertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener puertas disponibles", error = ex.Message });
            }
        }

        // GET: api/puertas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Puerta>> GetById(int id)
        {
            try
            {
                var puerta = await _puertaRepository.GetByIdAsync(id);

                if (puerta == null)
                    return NotFound(new { message = $"Puerta con ID {id} no encontrada" });

                return Ok(puerta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener la puerta", error = ex.Message });
            }
        }
    }
}