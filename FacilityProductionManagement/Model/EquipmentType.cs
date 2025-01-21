using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityProductionManagement.Model
{
    [Table("EquipmentType", Schema = "dbo")]
    public class EquipmentType
    {
        [Key]
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Area { get; set; }

        public ICollection<EquipmentContract> EquipmentContracts { get; set; }


    }
}
