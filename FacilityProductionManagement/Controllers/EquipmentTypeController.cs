using FacilityProductionManagement.Data;
using FacilityProductionManagement.DTOs;
using FacilityProductionManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityProductionManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EquipmentTypeController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentType/GetAll
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var equipmentTypes = _context.EquipmentTypes.ToList();
            var equipmentTypesDto = equipmentTypes.Select(e => new EquipmentDTO
            {
                Code = e.Code,
                Name = e.Name,
                Area = e.Area
            }).ToList();

            return Ok(equipmentTypesDto);
        }

        // POST: api/EquipmentType/Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] EquipmentDTO equipmentTypeDto)
        {
            if (equipmentTypeDto == null)
            {
                return BadRequest("Invalid data. Ensure all fields are filled with valid values.");
            }

            var existingEquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Code == equipmentTypeDto.Code);

            if (existingEquipmentType != null)
            {
                return Conflict($"Equipment type with code '{equipmentTypeDto.Code}' already exists.");
            }

            if (equipmentTypeDto.Area <= 0)
            {
                return BadRequest("Area must be greater than 0.");
            }

            var equipmentType = new EquipmentType
            {
                Code = equipmentTypeDto.Code,
                Name = equipmentTypeDto.Name,
                Area = equipmentTypeDto.Area
            };

            _context.EquipmentTypes.Add(equipmentType);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Equipment type with code '{equipmentType.Code}' created successfully." });
        }

        // DELETE: api/EquipmentType/Delete/{code}
        [HttpDelete]
        [Route("Delete/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Invalid code.");
            }

            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Code == code);

            if (equipmentType == null)
            {
                return NotFound($"Equipment type with code '{code}' not found.");
            }

            _context.EquipmentTypes.Remove(equipmentType);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Equipment type with code '{code}' deleted successfully." });
        }
    }
}
