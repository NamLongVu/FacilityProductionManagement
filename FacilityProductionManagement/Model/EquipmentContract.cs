using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityProductionManagement.Model
{
    [Table("EquipmentContract", Schema = "dbo")]
    public class EquipmentContract
    {
        [Key]
        public int Id { get; set; }
        public string FacilityCode { get; set; } = string.Empty;
        public string EquipmentTypeCode { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public Facility Facility { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}

