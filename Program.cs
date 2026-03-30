using Happy.Data;
using Happy.Repositories.Interfaces;
using Happy.Repositories.Implementations;
using Happy.Repositories.Interfaces.Admin;
using Happy.Repositories.Implementations.Admin;
using Happy.Services.Interfaces;
using Happy.Services.Implementations;
using Happy.Services.Interfaces.Admin;
using Happy.Services.Implementations.Admin;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add MVC
builder.Services.AddControllersWithViews();

// 🔹 Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        }));

// 🔹 Session
builder.Services.AddSession();

// 🔹 Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IAdminBookingRepository, AdminBookingRepository>();
builder.Services.AddScoped<IAdminRoomRepository, AdminRoomRepository>();

// 🔹 Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IAdminBookingService, AdminBookingService>();
builder.Services.AddScoped<IAdminRoomService, AdminRoomService>();

var app = builder.Build();

// 🔥 APPLY MIGRATION + SEED DATA (IMPORTANT FIX)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


// Ensure DB created
context.Database.Migrate();

    // Seed data
    DbInitializer.Seed(context);


}

// 🔹 Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// 🔹 Routing
app.MapControllerRoute(
name: "default",
pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
