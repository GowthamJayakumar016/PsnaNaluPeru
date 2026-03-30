using Happy.Data;
using Happy.Models;
using Happy.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Happy.Repositories.Implementations
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            var normalized = code.Trim();
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code == normalized);
        }
    }
}

