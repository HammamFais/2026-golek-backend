using Microsoft.EntityFrameworkCore;
using GolekBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrasi DbContext untuk PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Tambahkan layanan Controller
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 3. Map Controller agar API bisa diakses
app.MapControllers();

app.Run();