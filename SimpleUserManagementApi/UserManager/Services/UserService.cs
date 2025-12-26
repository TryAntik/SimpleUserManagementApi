using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;

namespace SimpleUserManagementApi.UserManager.Services;

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

    public async Task<UserDTO> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is null) throw new ArgumentException(  
            $"user with id {userId} not found");
        
        return new UserDTO(user.Id, user.Name, user.Email, user.CreatedAt);
    }

    public async Task<UserEntity> RegisterUserAsync(RegisterDTO request)
    {
        var userExists = await _userRepository.CheckUserExistsAsync(request.Email, request.Name);
        if (userExists) throw new Exception("user with same email, name is already registered.");

        var passwordHashed = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new UserEntity
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHashed
        };

        await _userRepository.AddUserAsync(user);
        return user;
    }

    public async Task<UserEntity> LoginUserAsync(LoginDTO request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        
        if(user is null) throw new NotFoundException($"user with email {request.Email} not found");

        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        
        if(!validPassword) throw new UnauthorizedAccessException($"invalid password");

        return user;
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

    public async Task UpdateUserAsync(Guid id, UpdateUserDTO userDTO)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if(user is null) throw new NotFoundException(
            $"user with id {id} not found");
        
        user.Name = userDTO.Name;
        user.Email = userDTO.Email;
        
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(Guid id)
        => await _userRepository.DeleteUserAsync(id);
}