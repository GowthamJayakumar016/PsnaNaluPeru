using System;

namespace Happy.DTOs.Payment
{
    public class ConfirmPaymentResultDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CouponCodeApplied { get; set; }
    }
}

