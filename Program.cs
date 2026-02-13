using GolekBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrasi DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// 2. AKTIFKAN SWAGGER STANDAR (Tanpa Gembok yang Bikin Error)
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
options.RoutePrefix = string.Empty; // Langsung muncul di halaman utama
});
}

app.UseHttpsRedirection();
app.UseAuthorization(); // Cukup Authorization saja, tanpa gembok JWT
app.MapControllers();

app.Run();