using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController(IPatientService patientService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await patientService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await patientService.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PatientDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await patientService.AddAsync(entity);

            return CreatedAtAction("GetById", new { id = result.PatientId }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await patientService.UpdateAsync(id, entity);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}