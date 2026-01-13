using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.RefreshToken;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.DataBase.Models;

namespace SimpleUserManagementApi.UserManager.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterRequestDTO requestDto);
    Task<LoginResponseDTO> LoginUserAsync(LoginRequestDTO request);
    Task<List<UserDTO>> GetAllUsersAsync(CancellationToken ct);
    Task<UserDTO?> GetUserByIdAsync(Guid id, CancellationToken ct);
    Task AddUserAsync(CreateUserDTO user);
    Task UpdateUserAsync(Guid id, UpdateUserDTO updatedUser);
    Task DeleteUserAsync(Guid id);
    Task<RefreshResponseDTO> RefreshTokenAsync(RefreshRequestDTO request, CancellationToken ct);
}