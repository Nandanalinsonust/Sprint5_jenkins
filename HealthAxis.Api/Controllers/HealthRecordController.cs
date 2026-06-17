using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService service;

        public HealthRecordController(IHealthRecordService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await service.GetByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var result = await service.GetByPatientIdAsync(patientId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HealthRecordDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AddAsync(entity);

            return CreatedAtAction(nameof(GetById),
                new { id = result.HealthRecordId },
                result);
        }
    }
}