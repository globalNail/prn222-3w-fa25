using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace Repository;

public class SystemUserRepository : GenericRepository<SystemUser>
{
    public SystemUserRepository()
    {
    }

    public SystemUserRepository(FA25EChargingDBDbContext context) => _context = context;

    public async Task<SystemUser> GetAccountAsync(string username, string password)
    {
        var result = await _context.SystemUsers
            .FirstOrDefaultAsync(predicate: sa => sa.Username == username && sa.UserPassword == password);

        if (result is not null)
        {
            return result;
        }

        return null!;
    }
}