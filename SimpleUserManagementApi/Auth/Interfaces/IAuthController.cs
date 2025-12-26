using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementApi.Auth.DTOs;

namespace SimpleUserManagementApi.Auth.Interfaces;

public interface IAuthController
{
    Task<ActionResult> Register(RegisterDTO request);
    Task<ActionResult> Login(LoginDTO request);
}