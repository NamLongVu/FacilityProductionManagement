using FacilityProductionManagement.Data;
using FacilityProductionManagement.DTOs;
using FacilityProductionManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityProductionManagement.Controllers
{
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly MyDbContext _context;
        public ContractController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/GetContracts")]
        public async Task<IActionResult> GetContracts()
        {
            var contracts = await _context.EquipmentContracts
                .Include(c => c.Facility)
                .Include(c => c.EquipmentType)
                .Select(c => new
                {
                    FacilityName = c.Facility.Name,
                    EquipmentTypeName = c.EquipmentType.Name,
                    c.Quantity
                })
                .ToListAsync();

            return Ok(contracts);
        }

        [HttpPost]
        [Route("api/CreateContract")]
        public async Task<IActionResult> CreateContract([FromBody] ContractDTO contractDto)
        {
            if (contractDto == null || string.IsNullOrWhiteSpace(contractDto.FacilityCode) ||
                string.IsNullOrWhiteSpace(contractDto.EquipmentTypeCode) || contractDto.Quantity <= 0)
            {
                return BadRequest("Invalid data. Ensure all fields are filled with valid values.");
            }

            var facility = await _context.Facilities
                .FirstOrDefaultAsync(f => f.Code == contractDto.FacilityCode);
            if (facility == null)
            {
                return NotFound($"Facility with code '{contractDto.FacilityCode}' not found.");
            }

            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Code == contractDto.EquipmentTypeCode);
            if (equipmentType == null)
            {
                return NotFound($"Equipment type with code '{contractDto.EquipmentTypeCode}' not found.");
            }

            var requiredArea = equipmentType.Area * contractDto.Quantity;

            var totalUsedArea = await _context.EquipmentContracts
                .Where(c => c.FacilityCode == facility.Code)
                .Join(_context.EquipmentTypes,
                      contract => contract.EquipmentTypeCode,
                      type => type.Code,
                      (contract, type) => type.Area * contract.Quantity)
                .SumAsync();

            var availableArea = facility.StandardArea - totalUsedArea;

            if (requiredArea > availableArea)
            {
                return BadRequest($"Insufficient area in the facility. Available area: {availableArea}, required: {requiredArea}.");
            }


            var newContract = new EquipmentContract
            {
                FacilityCode = contractDto.FacilityCode,
                EquipmentTypeCode = contractDto.EquipmentTypeCode,
                Quantity = contractDto.Quantity
            };
            _context.EquipmentContracts.Add(newContract);
            await _context.SaveChangesAsync();

            var response = new ContractDTO
            {
                FacilityCode = newContract.FacilityCode,
                EquipmentTypeCode = newContract.EquipmentTypeCode,
                Quantity = newContract.Quantity
            };

            return Ok(response);
        }
    }
}
