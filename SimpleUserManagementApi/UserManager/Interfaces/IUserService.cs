using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.RefreshToken;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterRequestDTO requestDto, CancellationToken ct);
    Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO request, CancellationToken ct);
    Task<List<UserDTO>> GetAllUsersAsync(CancellationToken ct);
    Task<UserDTO?> GetUserByIdAsync(Guid id, CancellationToken ct);
    Task AddUserAsync(CreateUserDTO user, CancellationToken ct);
    Task UpdateUserAsync(Guid id, UpdateUserDTO updatedUser, CancellationToken ct);
    Task DeleteUserAsync(Guid id, CancellationToken ct);
    Task<RefreshResponseDTO> RefreshTokenAsync(RefreshRequestDTO request, CancellationToken ct);
}