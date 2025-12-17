
using Castle.Core.Resource;
using HotelBooking.Infrastructure.Repositories;
using KlinikBooking.Core;
using KlinikBooking.Core.Entitites;
using KlinikBooking.Core.Interfaces;
using KlinikBooking.Infrastructure;
using KlinikBooking.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KlinikBookingContext>(opt => 
    opt.UseInMemoryDatabase("KlinikBookingDB"));

builder.Services.AddScoped<IRepository<TreatmentRoom>, TreatmentRoomRepository>();
builder.Services.AddScoped<IRepository<Patient>, PatientRepository>();
builder.Services.AddScoped<IRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddTransient<IDbinitializer, Dbinitializer>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialize the database.
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetService<KlinikBookingContext>();
        var dbInitializer = services.GetService<IDbinitializer>();
        dbInitializer.Initialize(dbContext);
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
