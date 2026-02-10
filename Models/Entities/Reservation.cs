namespace GolekBackend.Models.Entities;
public class Reservation
{
public int Id { get; set; }
public int RoomId { get; set; }
public int CustomerId { get; set; }
public DateTime StartTime { get; set; }
public DateTime EndTime { get; set; }
// Navigation Properties
public Room? Room { get; set; }
public Customer? Customer { get; set; }
}