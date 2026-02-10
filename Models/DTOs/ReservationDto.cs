namespace GolekBackend.Models.DTOs;
public class ReservationDto
{
public int Id { get; set; }
public int RoomId { get; set; }
public int CustomerId { get; set; }
public DateTime StartTime { get; set; }
public DateTime EndTime { get; set; }
public string? RoomName { get; set; }
public string? CustomerName { get; set; }
}

public class CreateReservationDto
{
public int RoomId { get; set; }
public int CustomerId { get; set; }
public DateTime StartTime { get; set; }
public DateTime EndTime { get; set; }
}