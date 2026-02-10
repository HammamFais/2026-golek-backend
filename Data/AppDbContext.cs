using Microsoft.EntityFrameworkCore;
using GolekBackend.Models.Entities;
namespace GolekBackend.Data;
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
public DbSet<Room> Rooms { get; set; }
public DbSet<Customer> Customers { get; set; }
public DbSet<Reservation> Reservations { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
// Data Seeding Ruangan
modelBuilder.Entity<Room>().HasData(
new Room { Id = 1, Name = "Lab Informatika 1", Capacity = 30, Status = "Available" },
new Room { Id = 2, Name = "Ruang Kelas A.201", Capacity = 40, Status = "Available" },
new Room { Id = 3, Name = "Aula Gedung TC", Capacity = 100, Status = "Occupied" }
);

// Data Seeding Customer
modelBuilder.Entity<Customer>().HasData(
new Customer { Id = 1, FullName = "Hammam Hidayatullah", Email = "hammam@pens.ac.id", PhoneNumber = "08123456789" },
new Customer { Id = 2, FullName = "Budi Santoso", Email = "budi@gmail.com", PhoneNumber = "08987654321" }
);

// Data Seeding Reservasi (Hammam pinjam Lab 1)
modelBuilder.Entity<Reservation>().HasData(
new Reservation { 
Id = 1, 
RoomId = 1, 
CustomerId = 1, 
StartTime = new DateTime(2026, 2, 10, 10, 0, 0, DateTimeKind.Utc), 
EndTime = new DateTime(2026, 2, 10, 12, 0, 0, DateTimeKind.Utc) 
}
);
}
}