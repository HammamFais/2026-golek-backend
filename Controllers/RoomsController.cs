using GolekBackend.Data;
using GolekBackend.Models.DTOs;
using GolekBackend.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolekBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoomsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
    {
        return await _context
            .Rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Status = r.Status,
            })
            .ToListAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<RoomDto>>> SearchRooms(
        [FromQuery] string? name,
        [FromQuery] int? minCapacity,
        [FromQuery] string? status
    )
    {
        // Kalau name kosong, langsung return kosong
        if (string.IsNullOrWhiteSpace(name))
        {
            return Ok(new List<RoomDto>());
        }

        // Tarik semua ke memory dulu, baru filter di C#
        var allRooms = await _context.Rooms.ToListAsync();

        var results = allRooms
            .Where(r => r.Name.ToLower().Contains(name.ToLower()))
            .Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Status = r.Status,
            })
            .ToList();

        if (minCapacity.HasValue)
            results = results.Where(r => r.Capacity >= minCapacity.Value).ToList();

        if (!string.IsNullOrEmpty(status))
            results = results.Where(r => r.Status.ToLower() == status.ToLower()).ToList();

        return Ok(results);
    }

    [HttpGet("report/status")]
    public async Task<IActionResult> GetRoomStatusReport()
    {
        var report = await _context
            .Rooms.GroupBy(r => r.Status)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count(),
                Rooms = g.Select(r => r.Name).ToList(),
            })
            .ToListAsync();

        return Ok(
            new
            {
                GeneratedAt = DateTime.UtcNow,
                TotalRooms = await _context.Rooms.CountAsync(),
                Details = report,
            }
        );
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto roomDto)
    {
        var room = new Room { Name = roomDto.Name, Capacity = roomDto.Capacity };
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return Ok(
            new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Status = room.Status,
            }
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, CreateRoomDto roomDto)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound("Ruangan ga ketemu King!");

        room.Name = roomDto.Name;
        room.Capacity = roomDto.Capacity;
        room.Status = roomDto.Status;

        await _context.SaveChangesAsync();
        return Ok(
            new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Status = room.Status,
            }
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return Ok("Ruangan dihapus, King!");
    }
}
