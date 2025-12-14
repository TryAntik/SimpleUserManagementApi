using SimpleUserManagementApi.CRUD.Repositories;
using SimpleUserManagementApi.CRUD.DTOs;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.Models;

namespace SimpleUserManagementApi.CRUD.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
        => _userRepository = userRepository;

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(a => new UserDTO(
            a.Id,
            a.Name,
            a.Email,
            a.CreatedAt)).ToList();
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null) throw new ArgumentException(
            $"user with id {userId} not found");
        
        return new UserDTO(user.Id, user.Name, user.Email, user.CreatedAt);
    }

    public async Task AddUserAsync(CreateUserDTO userDTO)
    {
        if (string.IsNullOrWhiteSpace(userDTO.Name) ||
            userDTO.Name.Contains(' '))
        {
            throw new ArgumentException("Name format is invalid");
        }

        var userEntity = new UserEntity
        {
            Name = userDTO.Name,
            Email = userDTO.Email
        };

        await _userRepository.AddUserAsync(userEntity);
    }

    public async Task UpdateUserAsync(int id, UpdateUserDTO userDTO)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if(user is null) throw new NotFoundException(
            $"user with id {id} not found");
        
        user.Name = userDTO.Name;
        user.Email = userDTO.Email;
        
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }
}