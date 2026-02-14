namespace GolekBackend.Models.DTOs;
public class RoomDto
{
public int Id { get; set; }
public string Name { get; set; } = string.Empty;
public int Capacity { get; set; }
public string Status { get; set; } = "Available";
}

public class CreateRoomDto
{
public string Name { get; set; } = string.Empty;
public int Capacity { get; set; }
}