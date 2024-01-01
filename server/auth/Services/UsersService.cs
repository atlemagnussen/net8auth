using Microsoft.EntityFrameworkCore;
using net8auth.auth.Data;
using net8auth.model;

namespace net8auth.auth.Services;

public class UsersService
{
    private readonly AuthDbContext _authDbContext;

    public UsersService(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }

    public async Task<List<ApplicationUser>> GetAll()
    {
        var users = await _authDbContext.Users.ToListAsync();
        return users;
    }
}