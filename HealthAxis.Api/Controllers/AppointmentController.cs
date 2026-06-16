using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
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

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AddAsync(entity);

            return CreatedAtAction("GetById",
                new { id = result.AppointmentId },
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.UpdateAsync(id, entity);

            if (result is null)
                return NotFound();

            return Ok(result);
        }
    }
}