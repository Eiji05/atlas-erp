using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.Common;
using AtlasERP.Application.DTOs.Auth;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Application.Interfaces.Services;
using AtlasERP.Domain.Entities;
using Microsoft.Extensions.Options;

namespace AtlasERP.Infrastructure.Services;

public class AuthService : IAuthService
{
    private const string DefaultRoleName = "Employee";

    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IOptions<JwtSettings> jwtOptions)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (await _authRepository.GetByUsernameAsync(dto.Username) != null)
            return false;

        if (await _authRepository.GetByEmailAsync(dto.Email) != null)
            return false;

        var role = await _authRepository.GetRoleByNameAsync(DefaultRoleName);

        if (role == null)
            throw new InvalidOperationException(
                $"Default role '{DefaultRoleName}' not found. Seed the Roles table first.");

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            FullName = dto.FullName,
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            RoleId = role.Id,
            IsActive = true
        };

        await _authRepository.AddAsync(user);
        await _authRepository.SaveChangesAsync();

        return true;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _authRepository.GetByUsernameAsync(dto.Username);

        if (user == null)
            return null;

        if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
            return null;

        return new AuthResponseDto
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            Username = user.Username,
            Role = user.Role?.Name ?? "",
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes)
        };
    }
}