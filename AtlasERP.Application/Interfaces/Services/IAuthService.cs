using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.DTOs.Auth;

namespace AtlasERP.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);

    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}
