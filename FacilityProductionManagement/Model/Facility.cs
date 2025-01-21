using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityProductionManagement.Model
{
    [Table("Facility", Schema = "dbo")]
    public class Facility
    {
        [Key]
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal StandardArea { get; set; }
        public ICollection<EquipmentContract> EquipmentContracts { get; set; }

    }
}
