using Microsoft.EntityFrameworkCore;
using WebAppDB.Models;

namespace WebAppDB.Services;
    public class UserService
{
    private readonly DatabaseFinalContext _context;

    public UserService(DatabaseFinalContext context)
    {
        _context = context;
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
    }
}
