using System;
using System.Collections.Generic;
using System.Text;
using AtlasERP.Domain.Entities;

namespace AtlasERP.Application.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}