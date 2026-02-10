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
Id = r.Id, RoomId = r.RoomId, CustomerId = r.CustomerId,
StartTime = r.StartTime, EndTime = r.EndTime,
RoomName = r.Room!.Name, CustomerName = r.Customer!.FullName
}).ToListAsync();
}

[HttpPost]
public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto dto)
{
var reservation = new Reservation {
RoomId = dto.RoomId, CustomerId = dto.CustomerId,
StartTime = dto.StartTime, EndTime = dto.EndTime
};
_context.Reservations.Add(reservation);
await _context.SaveChangesAsync();
return Ok(new ReservationDto { Id = reservation.Id, RoomId = reservation.RoomId, CustomerId = reservation.CustomerId });
}
}