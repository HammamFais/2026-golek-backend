using System.Text.Json;
using GolekBackend.Data;
using GolekBackend.Models.DTOs;
using GolekBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        return await _context
            .Reservations.Include(r => r.Room)
            .Include(r => r.Customer)
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomId = r.RoomId,
                CustomerId = r.CustomerId,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                RoomName = r.Room != null ? r.Room.Name : "Tanpa Nama",
                CustomerName = r.Customer != null ? r.Customer.FullName : "Anonim",
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> CreateReservation([FromBody] JsonElement data)
    {
        try
        {
            // Ambil data dengan cara yang rukun di .NET 8
            if (!data.TryGetProperty("roomId", out var roomIdProp))
                return BadRequest("roomId gaada, King!");

            if (!data.TryGetProperty("customerName", out var cNameProp))
                return BadRequest("customerName gaada, King!");

            int roomId = roomIdProp.GetInt32();
            string cName = cNameProp.GetString() ?? "Anonim";

            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound("Ruangan gaada King!");

            // CARI atau BUAT Customer baru biar ga error Foreign Key
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.FullName == cName);
            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = cName,
                    Email = cName.Replace(" ", "").ToLower() + "@pens.ac.id",
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var reservation = new Reservation
            {
                RoomId = roomId,
                CustomerId = customer.Id,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1),
            };

            room.Status = "Occupied";
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(
                new ReservationDto
                {
                    Id = reservation.Id,
                    RoomName = room.Name,
                    CustomerName = customer.FullName,
                }
            );
        }
        catch (Exception ex)
        {
            return BadRequest($"Waduh Error King: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        var res = await _context
            .Reservations.Include(r => r.Room)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (res == null)
            return NotFound();

        var room = res.Room;
        _context.Reservations.Remove(res);
        await _context.SaveChangesAsync();

        var anyLeft = await _context.Reservations.AnyAsync(r => r.RoomId == res.RoomId);
        if (!anyLeft && room != null)
        {
            room.Status = "Available";
            await _context.SaveChangesAsync();
        }

        return Ok("Data dihapus!");
    }
}
