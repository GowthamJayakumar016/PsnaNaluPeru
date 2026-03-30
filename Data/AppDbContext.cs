using Happy.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Happy.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

    // 🔹 Tables
    public DbSet<User> Users { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =============================
            // 🔹 RELATIONSHIPS
            // =============================

            // User → Hotel (Admin mapping)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Hotel)
                .WithMany(h => h.Admins)
                .HasForeignKey(u => u.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Hotel → Rooms
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId);

            // Room → Bookings
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);

            // User → Bookings
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Booking → Payment (1-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId);

            // Booking → Booking (rebooked_from self FK)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RebookedFromBooking)
                .WithMany()
                .HasForeignKey(b => b.RebookedFromBookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .Property(r => r.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.DiscountAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Coupon>()
                .Property(c => c.DiscountValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Room>()
                .HasIndex(r => new { r.HotelId, r.RoomNumber })
                .IsUnique();
        }
    }


}
