using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Services.DTOs.Auth;
using TodoApp.Services.Exceptions;
using TodoApp.Services.Interfaces;

namespace TodoApp.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUserRepository userRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new BusinessValidationException("Email та пароль є обов'язковими.");
        }

        var existingUser = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLower(), cancellationToken);

        if (existingUser != null)
        {
            throw new BusinessValidationException("Користувач з таким Email вже існує.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Email = dto.Email.Trim().ToLower(),
            PasswordHash = passwordHash,
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Token = token,
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new BusinessValidationException("Email та пароль є обов'язковими.");
        }

        var user = await _userRepository.GetByEmailAsync(dto.Email.Trim().ToLower(), cancellationToken);

        if (user == null)
        {
            throw new BusinessValidationException("Невірний email або пароль.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new BusinessValidationException("Невірний email або пароль.");
        }

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Token = token,
        };
    }
}