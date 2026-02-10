using Microsoft.EntityFrameworkCore;
using GolekBackend.Models.Entities;
namespace GolekBackend.Data;
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
public DbSet<Room> Rooms { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
// Data Seeding sesuai instruksi dosen
modelBuilder.Entity<Room>().HasData(
new Room { Id = 1, Name = "Lab Informatika 1", Capacity = 30, Status = "Available" },
new Room { Id = 2, Name = "Ruang Kelas A.201", Capacity = 40, Status = "Available" },
new Room { Id = 3, Name = "Aula Gedung TC", Capacity = 100, Status = "Occupied" }
);
}
}