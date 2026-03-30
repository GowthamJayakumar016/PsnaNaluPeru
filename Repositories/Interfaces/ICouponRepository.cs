using Happy.Models;
using System.Threading.Tasks;

namespace Happy.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByCodeAsync(string code);
    }
}

