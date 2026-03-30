using Happy.Data;
using Happy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Happy.Repositories.Implementations;
using Happy.Repositories.Interfaces.Admin;
using Happy.Repositories.Implementations.Admin;
using Happy.Services.Interfaces;
using Happy.Services.Implementations;
using Happy.Repositories.Implementations;
using Happy.Services.Interfaces.Admin;
using Happy.Services.Implementations.Admin;

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
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 🔹 Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IAdminBookingRepository, AdminBookingRepository>();
builder.Services.AddScoped<IAdminRoomRepository, AdminRoomRepository>();

// 🔹 Coupons
builder.Services.AddScoped<Happy.Repositories.Interfaces.ICouponRepository, Happy.Repositories.Implementations.CouponRepository>();

// 🔹 Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddScoped<IAdminBookingService, AdminBookingService>();
builder.Services.AddScoped<IAdminRoomService, AdminRoomService>();

// 🔹 Coupons
builder.Services.AddScoped<Happy.Services.Interfaces.ICouponService, Happy.Services.Implementations.CouponService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

// 🔹 Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

// 🔹 Routing
app.MapControllerRoute(
name: "default",
pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
