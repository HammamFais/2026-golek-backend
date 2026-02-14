using GolekBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrasi DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. AKTIFKAN SWAGGER STANDAR (Cara Paling Aman & Rukun)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Kita pake default aja biar gak error baris 17

var app = builder.Build();

// 3. KONFIGURASI SWAGGER UI (Ganti Nama di Sini Saja)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Ganti nama project-nya di bagian label sini, King!
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Golek Backend API");
        options.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();