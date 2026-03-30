using Happy.Models;
using Happy.Repositories.Interfaces;
using Happy.Services.Interfaces;

namespace Happy.Services.Implementations
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repo;

        public CouponService(ICouponRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Ok, Coupon? Coupon, decimal DiscountAmount, string? Error)> ValidateAndComputeAsync(string? code, decimal subtotal)
        {
            if (string.IsNullOrWhiteSpace(code))
                return (true, null, 0m, null);

            var normalized = code.Trim();

            var coupon = await _repo.GetByCodeAsync(normalized);
            if (coupon == null || !coupon.IsActive)
                return (false, null, 0m, "Invalid coupon code");

            if (coupon.ExpiryDate <= DateTime.UtcNow)
                return (false, null, 0m, "Coupon has expired");

            if (coupon.UsageLimit <= coupon.UsageCount)
                return (false, null, 0m, "Coupon usage limit reached");

            decimal discount;
            if (coupon.DiscountType == "flat")
            {
                discount = coupon.DiscountValue;
            }
            else if (coupon.DiscountType == "percentage")
            {
                discount = subtotal * (coupon.DiscountValue / 100m);
            }
            else
            {
                return (false, null, 0m, "Unsupported coupon type");
            }

            if (discount < 0) discount = 0;
            if (discount > subtotal) discount = subtotal;

            return (true, coupon, discount, null);
        }
    }
}

