using Microsoft.EntityFrameworkCore;
using RentAll_WebAPIs.Data;
using RentAll_WebAPIs.DTOs;
using RentAll_WebAPIs.Models;

namespace RentAll_WebAPIs.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;


        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await _context.Bookings.ToListAsync();

            return bookings.Select(MapToDto).ToList();
        }

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return null;

            return MapToDto(booking);
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(e => e.Id == dto.EquipmentId);

            if (equipment == null)
                throw new Exception("Equipment not found");

            if (!equipment.IsAvailable)
                throw new Exception("Equipment is not available");

            var totalDays =
                (dto.EndDate.Date - dto.StartDate.Date).Days + 1;

            var totalPrice = totalDays * equipment.DailyRate;

            var depositAmount = totalPrice * 0.20m;

            var booking = new Booking
            {
                EquipmentId = dto.EquipmentId,
                RenterName = dto.RenterName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalDays = totalDays,
                TotalPrice = totalPrice,
                DepositAmount = depositAmount,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

            var history = new BookingStatusHistory
            {
                BookingId = booking.Id,
                Status = "Pending"
            };

            _context.BookingStatusHistories.Add(history);

            await _context.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task<bool> UpdateBookingStatusAsync(int id, string status)
        {
            var booking = await _context.Bookings
                .Include(b => b.Equipment)   // add this
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return false;

            booking.Status = status;

            // Toggle availability based on status
            if (status == "Accepted" && booking.Equipment != null)
                booking.Equipment.IsAvailable = false;

            if (status == "Rejected" && booking.Equipment != null)
                booking.Equipment.IsAvailable = true;

            _context.BookingStatusHistories.Add(new BookingStatusHistory
            {
                BookingId = booking.Id,
                Status = status
            });

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<BookingResponseDto>>
GetBookingsByEquipmentAsync(int equipmentId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.EquipmentId == equipmentId)
                .ToListAsync();

            return bookings.Select(MapToDto).ToList();
        }

        public async Task<List<BookingResponseDto>>
GetBookingsByOwnerAsync(int ownerId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Equipment)
                .Where(b => b.Equipment.OwnerId == ownerId)
                .ToListAsync();

            return bookings.Select(MapToDto).ToList();
        }

        public async Task<bool> CancelBookingAsync(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Equipment)   // add this
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return false;

            booking.Status = "Cancelled";

            // Restore availability when cancelled
            if (booking.Equipment != null)
                booking.Equipment.IsAvailable = true;

            _context.BookingStatusHistories.Add(new BookingStatusHistory
            {
                BookingId = booking.Id,
                Status = "Cancelled"
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsEquipmentAvailableAsync(
    int equipmentId,
    DateTime startDate,
    DateTime endDate)
        {
            var conflict = await _context.Bookings
                .AnyAsync(b =>
                    b.EquipmentId == equipmentId &&
                    b.Status != "Cancelled" &&
                    startDate <= b.EndDate &&
                    endDate >= b.StartDate);

            return !conflict;
        }

        public async Task<List<BookingStatusHistory>>
GetHistoryByBookingAsync(int bookingId)
        {
            return await _context.BookingStatusHistories
                .Where(h => h.BookingId == bookingId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }


        private BookingResponseDto MapToDto(Booking booking)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                EquipmentId = booking.EquipmentId,
                RenterName = booking.RenterName,
                TotalPrice = booking.TotalPrice,
                DepositAmount = booking.DepositAmount,
                Status = booking.Status,
                StartDate = booking.StartDate,   
                EndDate = booking.EndDate
            };
        }
    }


}
