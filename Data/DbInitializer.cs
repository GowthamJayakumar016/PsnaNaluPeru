using Happy.Models;

namespace Happy.Data
{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // If already seeded → skip
            if (context.Hotels.Any())
                return;


        // ======================
        // 🔹 CREATE HOTELS
        // ======================
        var hotels = new List<Hotel>
        {
            new Hotel { Name = "Grand Palace", Location = "Chennai", Address = "Anna Salai", Phone="1111111111", Email="grand@hotel.com", IsActive = true },
            new Hotel { Name = "Sea View Resort", Location = "Goa", Address = "Beach Road", Phone="2222222222", Email="sea@hotel.com", IsActive = true },
            new Hotel { Name = "Hill Top Inn", Location = "Ooty", Address = "Hill Road", Phone="3333333333", Email="hill@hotel.com", IsActive = true },
            new Hotel { Name = "City Comfort", Location = "Bangalore", Address = "MG Road", Phone="4444444444", Email="city@hotel.com", IsActive = true },
            new Hotel { Name = "Royal Stay", Location = "Hyderabad", Address = "Banjara Hills", Phone="5555555555", Email="royal@hotel.com", IsActive = true }
        };

            context.Hotels.AddRange(hotels);
            context.SaveChanges();

            // ======================
            // 🔹 CREATE ROOMS
            // ======================
            var rooms = new List<Room>();

            foreach (var hotel in hotels)
            {
                rooms.Add(new Room { HotelId = hotel.Id, RoomNumber = "101", Type = "Single", Price = 1500, Capacity = 1, IsActive = true });
                rooms.Add(new Room { HotelId = hotel.Id, RoomNumber = "102", Type = "Double", Price = 2500, Capacity = 2, IsActive = true });
                rooms.Add(new Room { HotelId = hotel.Id, RoomNumber = "103", Type = "Deluxe", Price = 3500, Capacity = 3, IsActive = true });
            }

            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            // ======================
            // 🔹 CREATE ADMINS
            // ======================
            var admins = new List<User>();

            foreach (var hotel in hotels)
            {
                admins.Add(new User
                {
                    Name = hotel.Name + " Admin",
                    Email = hotel.Name.Replace(" ", "").ToLower() + "@admin.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin",
                    HotelId = hotel.Id
                });
            }

            context.Users.AddRange(admins);
            context.SaveChanges();
        }
    }


}
