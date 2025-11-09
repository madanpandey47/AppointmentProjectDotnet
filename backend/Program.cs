using backend.Data;
using backend.Repositories;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4000", policy =>
    {
        policy.WithOrigins("http://localhost:4000", "http://localhost:4001")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Repositories & Services
builder.Services.AddScoped<backend.Data.IUnitOfWork, UnitOfWork>(); // Add this line
builder.Services.AddScoped<backend.Repositories.IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Add controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("AllowLocalhost4000");
app.UseAuthorization();
app.MapControllers();
app.Run();
