using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.JWT;
using SimpleUserManagementApi.DataBase.Models;
using SimpleUserManagementApi.Exceptions;
using SimpleUserManagementApi.UserManager.DTOs;
using SimpleUserManagementApi.UserManager.Interfaces;

namespace SimpleUserManagementApi.UserManager.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

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

        if (user is null) throw new NotFoundException(  
            $"user with id {userId} not found");
        
        return new UserDTO(user.Id, user.Name, user.Email, user.CreatedAt);
    }

    public async Task RegisterUserAsync(RegisterDTO request)
    {
        var userExists = await _userRepository.CheckUserExistsAsync(request.Email);
        
        if (userExists) throw new Exception($"user with email {request.Email} is already registered");
        
        if (request.Name.Any(c => c == ' ')) throw new Exception("Name cannot contain spaces");
        if (request.Password.Any(c => c == ' ')) throw new Exception("Password cannot contain spaces");
        if (request.Email.Count(c => c == '.') != 1) throw new Exception("Invalid email format");
        
        var createUserDTO = new CreateUserDTO(
            request.Name,
            request.Email,
            BCrypt.Net.BCrypt.HashPassword(request.Password)
        );

        await AddUserAsync(createUserDTO);
    }
    
    public async Task<string> LoginUserAsync(LoginDTO request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if(user is null) throw new NotFoundException($"user with email {request.Email} not found");
        
        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if(!validPassword) throw new UnauthorizedAccessException($"invalid password"); 

        return _jwtService.GenerateToken(user);
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