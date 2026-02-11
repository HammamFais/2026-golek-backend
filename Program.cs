using GolekBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrasi DbContext untuk PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. AKTIFKAN SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. KONFIGURASI SWAGGER
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
options.RoutePrefix = string.Empty; // Biar Swagger langsung muncul di localhost:5016
});
}

app.UseAuthorization();
app.MapControllers();

app.Run();