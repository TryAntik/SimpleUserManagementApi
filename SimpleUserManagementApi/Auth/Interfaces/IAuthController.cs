using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;
using SimpleUserManagementApi.Auth.RefreshToken;

namespace SimpleUserManagementApi.Auth.Interfaces;

public interface IAuthController
{
    Task<ActionResult> Register(RegisterRequestDTO requestDto);
    Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO request);
    Task<ActionResult<RefreshResponseDTO>> RefreshToken(RefreshRequestDTO request);
}