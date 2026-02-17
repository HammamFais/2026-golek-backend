using GolekBackend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrasi DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Tambahkan CORS - Gerbang dibuka selebar mungkin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Gunakan CORS (Wajib di urutan ini!)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Golek Backend API");
        options.RoutePrefix = string.Empty; 
    });
}

// 4. MATIKAN HTTPS REDIRECTION (Biar gak bentrok sama Frontend localhost)
// app.UseHttpsRedirection(); // Saya matikan biar jalan tolnya lancar jaya

app.UseAuthorization();
app.MapControllers();

app.Run();