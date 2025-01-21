using FacilityProductionManagement.Data;
using FacilityProductionManagement.DTOs;
using FacilityProductionManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityProductionManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacilityController : ControllerBase
    {
        private readonly MyDbContext _context;

        public FacilityController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Facility/GetFacilities
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var facilities = _context.Facilities.ToList();
            var facilitiesDto = facilities.Select(f => new FacilityDTO
            {
                Code = f.Code,
                Name = f.Name,
                StandardArea = f.StandardArea,
            });
            return Ok(facilitiesDto);
        }

        // POST: api/Facility/Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] FacilityDTO facilityDto)
        {
            if (facilityDto == null || string.IsNullOrWhiteSpace(facilityDto.Code) || string.IsNullOrWhiteSpace(facilityDto.Name))
            {
                return BadRequest("Invalid data. Code and Name are required.");
            }

            if (facilityDto.StandardArea <= 0)
            {
                return BadRequest("StandardArea must be greater than 0.");
            }

            var existingFacility = await _context.Facilities.FirstOrDefaultAsync(f => f.Code == facilityDto.Code);
            if (existingFacility != null)
            {
                return Conflict($"Facility with code '{facilityDto.Code}' already exists.");
            }

            var newFacility = new Facility
            {
                Code = facilityDto.Code,
                Name = facilityDto.Name,
                StandardArea = facilityDto.StandardArea,
            };

            _context.Facilities.Add(newFacility);
            await _context.SaveChangesAsync();

            return Ok($"Facility with code '{facilityDto.Code}' created successfully.");
        }

        // DELETE: api/Facility/Delete/{code}
        [HttpDelete]
        [Route("Delete/{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Facility code is required.");
            }

            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Code == code);
            if (facility == null)
            {
                return NotFound($"Facility with code '{code}' not found.");
            }

            var hasContracts = await _context.EquipmentContracts.AnyAsync(c => c.FacilityCode == code);
            if (hasContracts)
            {
                return BadRequest($"Cannot delete facility '{code}' because it has associated equipment contracts.");
            }

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();

            return Ok($"Facility with code '{code}' deleted successfully.");
        }
    }
}
