using AutoMapper;
using HealthAxis.Api.Models.Dtos;
using HealthAxis.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService doctorservice,IMapper mapper) : ControllerBase
    {
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var result = await doctorservice.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await doctorservice.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DoctorDto entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await doctorservice.AddAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.Id }, result);
        }

        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await doctorservice.UpdateAsync(id,entity);
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
