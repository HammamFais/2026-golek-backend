using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolekBackend.Data;
using GolekBackend.Models.Entities;
using GolekBackend.Models.DTOs;

namespace GolekBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
private readonly AppDbContext _context;

public RoomsController(AppDbContext context)
{
_context = context;
}

[HttpGet]
public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
{
return await _context.Rooms
.Select(r => new RoomDto { Id = r.Id, Name = r.Name, Capacity = r.Capacity, Status = r.Status })
.ToListAsync();
}

[HttpGet("search")]
public async Task<ActionResult<IEnumerable<RoomDto>>> SearchRooms(
[FromQuery] int? minCapacity, 
[FromQuery] string? status)
{
var query = _context.Rooms.AsQueryable();

if (minCapacity.HasValue)
{
query = query.Where(r => r.Capacity >= minCapacity.Value);
}

if (!string.IsNullOrEmpty(status))
{
query = query.Where(r => r.Status.ToLower() == status.ToLower());
}

return await query
.Select(r => new RoomDto { Id = r.Id, Name = r.Name, Capacity = r.Capacity, Status = r.Status })
.ToListAsync();
}

[HttpGet("report/status")]
public async Task<IActionResult> GetRoomStatusReport()
{
var report = await _context.Rooms
.GroupBy(r => r.Status)
.Select(g => new {
Status = g.Key,
Count = g.Count(),
Rooms = g.Select(r => r.Name).ToList()
})
.ToListAsync();

return Ok(new {
GeneratedAt = DateTime.UtcNow,
TotalRooms = await _context.Rooms.CountAsync(),
Details = report
});
}

[HttpPost]
public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto roomDto)
{
var room = new Room { Name = roomDto.Name, Capacity = roomDto.Capacity };
_context.Rooms.Add(room);
await _context.SaveChangesAsync();
return Ok(new RoomDto { Id = room.Id, Name = room.Name, Capacity = room.Capacity, Status = room.Status });
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateRoom(int id, CreateRoomDto roomDto)
{
var room = await _context.Rooms.FindAsync(id);
if (room == null) return NotFound();
room.Name = roomDto.Name;
room.Capacity = roomDto.Capacity;
await _context.SaveChangesAsync();
return NoContent();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteRoom(int id)
{
var room = await _context.Rooms.FindAsync(id);
if (room == null) return NotFound();
_context.Rooms.Remove(room);
await _context.SaveChangesAsync();
return NoContent();
}
}