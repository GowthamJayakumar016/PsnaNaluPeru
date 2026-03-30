using Happy.Models;
using Microsoft.EntityFrameworkCore;

namespace Happy.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // Seed Hotels/Rooms/Users (idempotent).
            // We seed the base dataset and then add missing resorts/rooms even if the DB already contains data.
            var hotelSeeds = new (string Name, string Location, string Address, string Phone, string Email, string AdminEmail, string AdminName)[] {
                ("Nalu Beach Resort","Lima Coast","Km 42 Panamericana Sur","+51 1 555 0101","frontdesk@nalu.demo","admin@nalu.demo","Nalu Admin"),
                ("Psna City Inn","Cusco Center","Av. Sol 120","+51 84 555 0202","hola@psna.demo","admin@psna.demo","Psna Admin"),
                ("Coral Reef Suites","Trujillo Bay","Av. Costera 55","+51 44 555 0303","contact@coral.demo","admin@coral.demo","Coral Admin"),
                ("Andes Mountain Lodge","Cusco Highlands","Carretera a Ollantaytambo 12","+51 84 555 0404","stay@andes.demo","admin@andes.demo","Andes Admin"),
                ("Solterra Boutique","Miraflores","Calle Sol 88","+51 1 555 0505","hello@solterra.demo","admin@solterra.demo","Solterra Admin"),
                ("Amazon River Retreat","Iquitos Riverbank","Jr. Amazonas 21","+51 65 555 0606","frontdesk@amazon.demo","admin@amazon.demo","Amazon Admin"),
                ("Atacama Desert Resort","Tacna Desert Edge","Ruta 12 km 7","+51 52 555 0707","info@atacama.demo","admin@atacama.demo","Atacama Admin")
            };

            var hotelsByName = new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var hs in hotelSeeds)
            {
                var hotel = await db.Hotels.FirstOrDefaultAsync(h => h.Name == hs.Name);
                if (hotel == null)
                {
                    hotel = new Hotel
                    {
                        Name = hs.Name,
                        Location = hs.Location,
                        Address = hs.Address,
                        Phone = hs.Phone,
                        Email = hs.Email,
                        IsActive = true
                    };
                    db.Hotels.Add(hotel);
                    await db.SaveChangesAsync();
                }

                hotelsByName[hs.Name] = hotel.Id;
            }

            // Rooms (idempotent by HotelId + RoomNumber).
            var roomSeeds = new (string HotelName, string RoomNumber, string Type, string Amenities, decimal Price, int Capacity, bool IsActive)[] {
                // Nalu Beach Resort
                ("Nalu Beach Resort","101","Deluxe","WiFi, AC, Breakfast",120,2,true),
                ("Nalu Beach Resort","102","Standard","WiFi, Fan",85,2,true),
                ("Nalu Beach Resort","103","Standard","WiFi, Fan, Balcony",90,2,true),
                ("Nalu Beach Resort","104","Family","WiFi, AC, Breakfast, Kitchenette",150,4,true),

                // Psna City Inn
                ("Psna City Inn","201","Suite","WiFi, AC, Kitchen",200,4,true),
                ("Psna City Inn","202","Standard","WiFi, Fan",95,2,true),
                ("Psna City Inn","203","Deluxe","WiFi, AC, Breakfast",140,3,true),
                ("Psna City Inn","204","Family","WiFi, AC, Kitchen",160,4,true),

                // Coral Reef Suites
                ("Coral Reef Suites","301","Suite","WiFi, AC, OceanView, Kitchen",220,4,true),
                ("Coral Reef Suites","302","Deluxe","WiFi, AC, Breakfast",135,3,true),
                ("Coral Reef Suites","303","Standard","WiFi, Fan",100,2,true),
                ("Coral Reef Suites","304","Deluxe","WiFi, AC, Balcony",145,3,true),

                // Andes Mountain Lodge
                ("Andes Mountain Lodge","401","Standard","WiFi, Heater, MountainView",110,2,true),
                ("Andes Mountain Lodge","402","Deluxe","WiFi, Heater, Breakfast",160,3,true),
                ("Andes Mountain Lodge","403","Suite","WiFi, Fireplace, Kitchen",240,4,true),
                ("Andes Mountain Lodge","404","Deluxe","WiFi, Heater, Balcony",170,3,true),

                // Solterra Boutique
                ("Solterra Boutique","501","Standard","WiFi, AC, CityView",105,2,true),
                ("Solterra Boutique","502","Deluxe","WiFi, AC, Breakfast",155,3,true),
                ("Solterra Boutique","503","Suite","WiFi, AC, Terrace",210,4,true),
                ("Solterra Boutique","504","Standard","WiFi, AC",115,2,true),

                // Amazon River Retreat
                ("Amazon River Retreat","601","Standard","WiFi, Fan, RiverView",115,2,true),
                ("Amazon River Retreat","602","Deluxe","WiFi, AC, Breakfast",170,3,true),
                ("Amazon River Retreat","603","Suite","WiFi, AC, Kitchen, RiverView",260,4,true),
                ("Amazon River Retreat","604","Family","WiFi, AC, Kitchen",230,4,true),

                // Atacama Desert Resort
                ("Atacama Desert Resort","701","Standard","WiFi, AC, DesertView",120,2,true),
                ("Atacama Desert Resort","702","Deluxe","WiFi, AC, Breakfast",180,3,true),
                ("Atacama Desert Resort","703","Suite","WiFi, AC, Kitchen, Stargazing",250,4,true),
                ("Atacama Desert Resort","704","Family","WiFi, AC, Breakfast, Kitchenette",200,4,true)
            };

            foreach (var rs in roomSeeds)
            {
                if (!hotelsByName.TryGetValue(rs.HotelName, out var hotelId))
                    continue;

                var exists = await db.Rooms.AnyAsync(r => r.HotelId == hotelId && r.RoomNumber == rs.RoomNumber);
                if (exists)
                    continue;

                db.Rooms.Add(new Room
                {
                    HotelId = hotelId,
                    RoomNumber = rs.RoomNumber,
                    Type = rs.Type,
                    Amenities = rs.Amenities,
                    Price = rs.Price,
                    Capacity = rs.Capacity,
                    IsActive = rs.IsActive
                });
            }
            await db.SaveChangesAsync();

            // Users (idempotent by email)
            async Task EnsureUserAsync(string name, string email, string password, string role, int? hotelId)
            {
                var existing = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existing != null)
                    return;

                db.Users.Add(new User
                {
                    Name = name,
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = role,
                    HotelId = hotelId
                });
            }

            await EnsureUserAsync("Demo Guest", "guest@demo.com", "Guest123!", "User", null);

            // Create admin users for each seeded hotel.
            foreach (var hs in hotelSeeds)
            {
                await EnsureUserAsync(hs.AdminName, hs.AdminEmail, "Admin123!", "Admin", hotelsByName[hs.Name]);
            }

            await db.SaveChangesAsync();

            // Seed Coupons if missing.
            if (!await db.Coupons.AnyAsync())
            {
                db.Coupons.AddRange(
                    new Coupon
                    {
                        Code = "WELCOME10",
                        DiscountType = "percentage",
                        DiscountValue = 10,
                        ExpiryDate = DateTime.UtcNow.AddMonths(6),
                        UsageLimit = 1000,
                        UsageCount = 0,
                        IsActive = true
                    },
                    new Coupon
                    {
                        Code = "FLAT50",
                        DiscountType = "flat",
                        DiscountValue = 50,
                        ExpiryDate = DateTime.UtcNow.AddMonths(6),
                        UsageLimit = 500,
                        UsageCount = 0,
                        IsActive = true
                    }
                );
                await db.SaveChangesAsync();
            }

            // Backfill room amenities if the column exists but was empty in an older DB.
            var anyEmptyAmenities = await db.Rooms.AnyAsync(r => string.IsNullOrWhiteSpace(r.Amenities));
            if (anyEmptyAmenities)
            {
                foreach (var room in db.Rooms)
                {
                    if (!string.IsNullOrWhiteSpace(room.Amenities))
                        continue;

                    // Minimal backfill based on room number (matches our seeded demo data).
                    room.Amenities = room.RoomNumber switch
                    {
                        "101" => "WiFi, AC, Breakfast",
                        "102" => "WiFi, Fan",
                        "201" => "WiFi, AC, Kitchen",
                        "202" => "WiFi, Fan",
                        _ => "WiFi"
                    };
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
