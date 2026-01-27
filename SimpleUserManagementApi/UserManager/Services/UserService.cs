using System.Security.Cryptography;
using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.JWT;
using SimpleUserManagementApi.Auth.RefreshToken;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IUserRepository userRepository,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _logger = logger;
    }

    public async Task<List<UserDTO>> GetAllUsersAsync(CancellationToken ct)
    {
        var users = await _userRepository.GetAllUsersAsync(ct);
        
        return users.Select(a => new UserDTO(
            a.Id,
            a.Name,
            a.Email,
            a.CreatedAt)).ToList();
    }

    public async Task<UserDTO> GetUserByIdAsync(Guid userId, CancellationToken ct)
    {
        var user = await _userRepository.GetUserByIdAsync(userId, ct);

        if (user is null)
        {
            _logger.LogInformation("User with id {UserId} not found", userId);
            throw new NotFoundException($"User with id {userId} not found");
        }
        
        return new UserDTO(user.Id, user.Name, user.Email, user.CreatedAt);
    }

    public async Task RegisterUserAsync(RegisterRequestDTO requestDTO, CancellationToken ct)
    {
        if (await _userRepository.CheckUserExistsAsync(requestDTO.Email, ct))
        {
            _logger.LogWarning("User with email {Email} is already registered", requestDTO.Email);
            throw new InvalidOperationException($"User with email {requestDTO.Email} is already registered");
        }
        
        var createUserDTO = new CreateUserDTO(
            requestDTO.Email.ToLowerInvariant().Trim(),
            requestDTO.Name.Trim(),
            requestDTO.Password
        );

        await AddUserAsync(createUserDTO, ct);
    }

    public async Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO request, CancellationToken ct)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, ct);
        if (user is null)
        {
            _logger.LogWarning("User with email {Email} not found", request.Email);
            throw new NotFoundException($"user with email {request.Email} not found");
        }
        
        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!validPassword)
        {
            _logger.LogWarning("Password {Password} is incorrect", request.Password);
            throw new UnauthorizedAccessException($"Password is incorrect");
        }
        
        var accessToken = _jwtService.GenerateToken(user);
        var refreshToken = await _refreshTokenService.CreateTokenAsync(user.Id, ct);
        
        return new LoginResponseDTO(accessToken, refreshToken);
    }

    public async Task AddUserAsync(CreateUserDTO userDTO, CancellationToken ct)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

        if (await _userRepository.CheckUserExistsAsync(userDTO.Email, ct))
        {
            _logger.LogWarning("User with email {Email} is alreadyt registered", userDTO.Email);
            throw new InvalidOperationException($"User with email {userDTO.Email} is already registered");
        }
        
        var userEntity = new UserEntity
        {
            Name = userDTO.Name,
            Email = userDTO.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.AddUserAsync(userEntity, ct); 
    }

    public async Task UpdateUserAsync(Guid id, UpdateUserDTO userDTO, CancellationToken ct)
    {
        var user = await _userRepository.GetUserByIdAsync(id, ct);
        
        if(user is null) {
            _logger.LogWarning("User with id {UserId} not found", id);
            throw new NotFoundException($"User with id {id} not found");
        }
        user.Name = userDTO.Name;
        user.Email = userDTO.Email;
        
        await _userRepository.UpdateUserAsync(user, ct);
    }

    public async Task<RefreshResponseDTO> RefreshTokenAsync(RefreshRequestDTO request, CancellationToken ct)
    {
        var refreshEntity = await _refreshTokenService.GetTokenAsync(request.RefreshTokenDto, ct);
        if (refreshEntity is null)
        {
            _logger.LogWarning("Refresh token {RefreshToken} is invalid", request.RefreshTokenDto);
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        await _refreshTokenService.RevokeTokenAsync(refreshEntity.Id, ct);

        var userEntity = await _userRepository.GetUserByIdAsync(refreshEntity.UserId, ct);
        if (userEntity is null)
        {
            _logger.LogWarning("User with id {UserId} not found", refreshEntity.UserId);
            throw new NotFoundException($"User with id {refreshEntity.UserId} was not found");
        }

        var newJwt = _jwtService.GenerateToken(userEntity);
        var newRefreshToken = await _refreshTokenService.CreateTokenAsync(userEntity.Id, ct);
        
        return new RefreshResponseDTO(newJwt, newRefreshToken.Token);
    }

    public async Task DeleteUserAsync(Guid id, CancellationToken ct)
        => await _userRepository.DeleteUserAsync(id, ct);
}