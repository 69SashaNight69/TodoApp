using TodoApp.Services.DTOs.Auth;

namespace TodoApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    }
}