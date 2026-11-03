using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Domain.Entities;

namespace AtlasERP.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<Role?> GetRoleByNameAsync(string name);
    Task AddAsync(User user);
    Task SaveChangesAsync();   
}