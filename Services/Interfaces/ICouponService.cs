using System.Threading.Tasks;
using Happy.Models;

namespace Happy.Services.Interfaces
{
    public interface ICouponService
    {
        Task<(bool Ok, Coupon? Coupon, decimal DiscountAmount, string? Error)> ValidateAndComputeAsync(string? code, decimal subtotal);
    }
}

