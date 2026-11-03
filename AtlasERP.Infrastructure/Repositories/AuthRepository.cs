using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Application.Interfaces.Repositories;
using AtlasERP.Domain.Entities;
using AtlasERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtlasERP.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AtlasDbContext _context;

    public AuthRepository(AtlasDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}