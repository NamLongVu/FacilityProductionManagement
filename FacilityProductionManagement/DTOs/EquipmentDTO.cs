using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityProductionManagement.DTOs
{
    public class EquipmentDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Area { get; set; }
    }
}
