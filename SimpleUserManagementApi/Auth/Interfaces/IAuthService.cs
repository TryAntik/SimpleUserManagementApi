using SimpleUserManagementApi.Auth.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SimpleUserManagementApi.Auth.Interfaces;

public interface IAuthService
{
    Task<ActionResult> Register(RegisterDTO request);
}