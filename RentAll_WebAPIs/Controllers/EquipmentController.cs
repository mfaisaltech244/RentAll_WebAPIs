using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAll_WebAPIs.DTOs;
using RentAll_WebAPIs.Models;
using RentAll_WebAPIs.Services;

namespace RentAll_WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;
        private readonly FileService _fileService;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        public EquipmentController(IEquipmentService equipmentService, FileService fileService)
        {
            _equipmentService = equipmentService;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipment>>> GetAll()
        {
            return await _equipmentService.GetAllEquipmentAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetById(int id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id);

            if (equipment == null)
                return NotFound();

            return equipment;
        }

        [HttpPost]
        [Consumes("Multipart/form-data")]
        public async Task<IActionResult> AddEquipment([FromForm] EquipmentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest(new { message = "Image is required." });

            var ext = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                return BadRequest(new { message = "Only .jpg, .jpeg, .png, .webp files are allowed." });

            if (dto.Image.Length > MaxFileSizeBytes)
                return BadRequest(new { message = "File size exceeds the 5 MB limit." });

            var imageUrl = await _fileService.SaveFileAsync(dto.Image);
            var equipment = await _equipmentService.AddEquipmentAsync(dto, imageUrl);

            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, equipment);
        }


        [HttpPut("{id:int}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateEquipment(
            int id,
            [FromForm] EquipmentUpdateDto dto,
            IFormFile? image)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(ext))
                    return BadRequest(new { message = "Only .jpg, .jpeg, .png, .webp files are allowed." });

                if (image.Length > MaxFileSizeBytes)
                    return BadRequest(new { message = "File size exceeds the 5 MB limit." });

                var existing = await _equipmentService.GetEquipmentByIdAsync(id);
                if (existing != null && !string.IsNullOrEmpty(existing.ImageUrl))
                    _fileService.DeleteFile(existing.ImageUrl);

                imageUrl = await _fileService.SaveFileAsync(image);
            }

            var updated = await _equipmentService.UpdateEquipmentAsync(id, dto, imageUrl);
            if (!updated)
                return NotFound(new { message = $"Equipment with ID {id} not found." });

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var existing = await _equipmentService.GetEquipmentByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Equipment with ID {id} not found." });

            _fileService.DeleteFile(existing.ImageUrl);

            await _equipmentService.DeleteEquipmentAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetAvailabilityStatus(int id)
        {
            var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
            if (equipment == null)
                return NotFound(new { message = $"Equipment with ID {id} not found." });

            var status = await _equipmentService.GetAvailabilityStatusAsync(id);
            return Ok(new { equipmentId = id, status });
        }

        [HttpPatch("{id:int}/availability")]
        [Authorize]
        public async Task<IActionResult> SetAvailability(int id, [FromBody] SetAvailabilityDto dto)
        {
            var updated = await _equipmentService.SetAvailabilityAsync(id, dto.IsAvailable);
            if (!updated)
                return NotFound(new { message = $"Equipment with ID {id} not found." });

            return Ok(new { equipmentId = id, isAvailable = dto.IsAvailable });
        }
    }
}
