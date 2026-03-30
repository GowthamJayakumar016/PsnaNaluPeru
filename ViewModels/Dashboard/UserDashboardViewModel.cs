using System.Collections.Generic;
using Happy.DTOs.Booking;

namespace Happy.ViewModels.Dashboard
{
    public class UserDashboardViewModel
    {
        public List<BookingViewDto> MyBookings { get; set; }


    public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }

        public int ConfirmedBookings { get; set; }
    }


}
