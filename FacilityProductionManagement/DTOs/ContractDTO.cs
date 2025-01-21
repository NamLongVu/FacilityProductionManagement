namespace FacilityProductionManagement.DTOs
{
    public class ContractDTO
    {
        public string FacilityCode { get; set; } = string.Empty;
        public string EquipmentTypeCode { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
