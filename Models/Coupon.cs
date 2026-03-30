using System;

namespace Happy.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        public string Code { get; set; }

        // "flat" => discount_value is an absolute amount
        // "percentage" => discount_value is a percentage (e.g., 10 => 10%)
        public string DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int UsageLimit { get; set; }
        public int UsageCount { get; set; }

        public bool IsActive { get; set; }
    }
}

