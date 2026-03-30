using System.ComponentModel.DataAnnotations;

namespace Happy.DTOs.Payment
{
    public class PaymentConfirmDto
    {
        [Required]
        public string Method { get; set; }

        public string? CouponCode { get; set; }
    }
}

