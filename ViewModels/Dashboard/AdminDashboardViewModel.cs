using System.Collections.Generic;
using Happy.DTOs.Admin;

namespace Happy.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        public List<AdminBookingDto> Bookings { get; set; }


    public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }

        public int ConfirmedBookings { get; set; }
    }


}
