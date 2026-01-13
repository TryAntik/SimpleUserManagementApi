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

    public UserService(IUserRepository userRepository, IJwtService jwtService, IRefreshTokenService refreshTokenService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
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

        if (user is null) throw new NotFoundException(  
            $"user with id {userId} not found");
        
        return new UserDTO(user.Id, user.Name, user.Email, user.CreatedAt);
    }

    public async Task RegisterUserAsync(RegisterRequestDTO requestDto)
    {
        var userExists = await _userRepository.CheckUserExistsAsync(requestDto.Email);
        
        if (userExists) throw new Exception($"user with email {requestDto.Email} is already registered");
        
        if (requestDto.Name.Any(c => c == ' ')) throw new Exception("Name cannot contain spaces");
        if (requestDto.Password.Any(c => c == ' ')) throw new Exception("Password cannot contain spaces");
        if (requestDto.Email.Count(c => c == '.')  != 1 ||
            requestDto.Email.Count(c => c == '@') != 1) throw new Exception("Invalid email format");
        
        var createUserDTO = new CreateUserDTO(
            requestDto.Name,
            requestDto.Email,
            BCrypt.Net.BCrypt.HashPassword(requestDto.Password)
        );

        await AddUserAsync(createUserDTO);
    }
    
    public async Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, default);
        if(user is null) throw new NotFoundException($"user with email {request.Email} not found");
        
        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!validPassword) throw new UnauthorizedAccessException($"invalid password");
        
        var accessToken = _jwtService.GenerateToken(user);
        var refreshTokenEntity = await _refreshTokenService.CreateTokenAsync(user.Id);
        
        return new LoginResponseDTO(accessToken, refreshTokenEntity.Token);
    }

    public async Task AddUserAsync(CreateUserDTO userDTO)
    {
        var userEntity = new UserEntity
        {
            Name = userDTO.Name,
            Email = userDTO.Email,
            PasswordHash = userDTO.PasswordHash
        };

        await _userRepository.AddUserAsync(userEntity); 
    }

    public async Task UpdateUserAsync(Guid id, UpdateUserDTO userDTO)
    {
        var user = await _userRepository.GetUserByIdAsync(id, default);
        
        if(user is null) throw new NotFoundException(
            $"user with id {id} not found");
        
        user.Name = userDTO.Name;
        user.Email = userDTO.Email;
        
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task<RefreshResponseDTO> RefreshTokenAsync(RefreshRequestDTO request, CancellationToken ct)
    {
        var userId = _jwtService.GetUserIdFromToken(request.AccessToken); 
        var currentRefreshToken = request.RefreshToken;
        
        var isValidRefreshToken = await _refreshTokenService.IsValidTokenAsync(currentRefreshToken, userId);
        if (!isValidRefreshToken) throw new UnauthorizedAccessException("Invalid refresh token");
        
        var refreshEntity = await _refreshTokenService.GetTokenAsync(currentRefreshToken, userId);
        if (refreshEntity is null) throw new UnauthorizedAccessException("Invalid refresh token");

        await _refreshTokenService.RevokeTokenAsync(refreshEntity.Id);

        var userEntity = await _userRepository.GetUserByIdAsync(userId, ct);
        if(userEntity is null) throw new NotFoundException($"user with id {userId} not found");
        
        var newAccessToken = _jwtService.GenerateToken(userEntity);
        var newRefreshToken = await _refreshTokenService.CreateTokenAsync(userId);
        
        return new RefreshResponseDTO(newAccessToken, newRefreshToken.Token);
    }

    public async Task DeleteUserAsync(Guid id)
        => await _userRepository.DeleteUserAsync(id);
}