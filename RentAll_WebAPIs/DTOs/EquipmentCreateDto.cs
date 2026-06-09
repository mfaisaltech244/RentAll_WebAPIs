using System.ComponentModel.DataAnnotations;

namespace RentAll_WebAPIs.DTOs
{
    public class EquipmentCreateDto
    {
        [Required(ErrorMessage = "Owner ID is required.")]
        public int OwnerId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Daily rate is required.")]
        [Range(1, 999999, ErrorMessage = "Daily rate must be between 1 and 999,999.")]
        public decimal DailyRate { get; set; }

        public bool IsAvailable { get; set; } = true;
    }

    public class EquipmentUpdateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Daily rate is required.")]
        [Range(1, 999999, ErrorMessage = "Daily rate must be between 1 and 999,999.")]
        public decimal DailyRate { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
    public class EquipmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal DailyRate { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
