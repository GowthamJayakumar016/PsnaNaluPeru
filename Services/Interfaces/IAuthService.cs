using Happy.DTOs.Auth;

namespace Happy.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        public Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}
