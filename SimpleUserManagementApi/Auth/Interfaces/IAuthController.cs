using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;

namespace SimpleUserManagementApi.Auth.Interfaces;

public interface IAuthController
{
    Task<ActionResult> Register(RegisterRequestDTO requestDto);
    Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO request);
}