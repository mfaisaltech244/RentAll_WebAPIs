namespace RentAll_WebAPIs.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }

        public int EquipmentId { get; set; }

        public string RenterName { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public decimal DepositAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }  
        public DateTime EndDate { get; set; }  
    }
}