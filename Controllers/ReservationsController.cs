using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolekBackend.Data;
using GolekBackend.Models.Entities;
using GolekBackend.Models.DTOs;

namespace GolekBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
private readonly AppDbContext _context;
public ReservationsController(AppDbContext context) => _context = context;

[HttpGet]
public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
{
return await _context.Reservations
.Include(r => r.Room).Include(r => r.Customer)
.Select(r => new ReservationDto {
Id = r.Id, 
RoomId = r.RoomId, 
CustomerId = r.CustomerId,
StartTime = r.StartTime, 
EndTime = r.EndTime,
RoomName = r.Room!.Name, 
CustomerName = r.Customer!.FullName
}).ToListAsync();
}

[HttpPost]
public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto dto)
{
// 1. Cek apakah Ruangan ada
var room = await _context.Rooms.FindAsync(dto.RoomId);
if (room == null) return NotFound("Ruangan tidak ditemukan, King!");

// 2. LOGIKA SAKTI: Cek Bentrok Jadwal
var isBentrok = await _context.Reservations
.AnyAsync(r => r.RoomId == dto.RoomId && 
((dto.StartTime >= r.StartTime && dto.StartTime < r.EndTime) || 
(dto.EndTime > r.StartTime && dto.EndTime <= r.EndTime) ||
(dto.StartTime <= r.StartTime && dto.EndTime >= r.EndTime)));

if (isBentrok) return BadRequest("Waduh King, jam segitu ruangannya sudah ada yang booking!");

var reservation = new Reservation {
RoomId = dto.RoomId, 
CustomerId = dto.CustomerId,
StartTime = dto.StartTime, 
EndTime = dto.EndTime
};

// 3. Otomatis update status ruangan jadi Occupied
room.Status = "Occupied";

_context.Reservations.Add(reservation);
await _context.SaveChangesAsync();

return Ok(new ReservationDto { 
Id = reservation.Id, 
RoomId = reservation.RoomId, 
CustomerId = reservation.CustomerId,
RoomName = room.Name,
CustomerName = "Reservasi Berhasil & Aman dari Bentrok!"
});
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteReservation(int id)
{
// Amankan data reservasi beserta info ruangannya
var reservation = await _context.Reservations
.Include(r => r.Room)
.FirstOrDefaultAsync(r => r.Id == id);

if (reservation == null) return NotFound("Data reservasi nggak ketemu, King!");

// Kembalikan status ruangan jadi Available lagi
if (reservation.Room != null)
{
reservation.Room.Status = "Available";
}

_context.Reservations.Remove(reservation);
await _context.SaveChangesAsync();

return Ok("Reservasi berhasil dicancel, ruangan sekarang kosong lagi!");
}
}