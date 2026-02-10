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
public RoomsController(AppDbContext context) => _context = context;

[HttpGet]
public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
{
return await _context.Rooms
.Select(r => new RoomDto { Id = r.Id, Name = r.Name, Capacity = r.Capacity, Status = r.Status })
.ToListAsync();
}

[HttpPost]
public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomDto roomDto)
{
var room = new Room { Name = roomDto.Name, Capacity = roomDto.Capacity };
_context.Rooms.Add(room);
await _context.SaveChangesAsync();
return Ok(new RoomDto { Id = room.Id, Name = room.Name, Capacity = room.Capacity, Status = room.Status });
}
}