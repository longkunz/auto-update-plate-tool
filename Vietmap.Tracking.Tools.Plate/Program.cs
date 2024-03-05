using Serilog;
using System.Diagnostics;
using Vietmap.Tracking.Tools.Plate.DbContexts;
using Vietmap.Tracking.Tools.Plate.Repositories;
using Vietmap.Tracking.Tools.Plate.Services;

var builder = WebApplication.CreateBuilder(args);
Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
Serilog.Debugging.SelfLog.Enable(Console.Error);
// Add services to the container.
builder.Services.AddSingleton<DapperDbContext>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddTransient<IVehicleService, VehicleService>();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
