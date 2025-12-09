using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;

namespace SCMS.Repository.TienPVK.Implements;

public class SystemAccountRepository : GenericRepository<SystemAccount>
{
    public SystemAccountRepository()
    {
    }

    public SystemAccountRepository(FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext context) => _context = context;

    public async Task<SystemAccount> GetAccountAsync(string username, string password)
    {
        return await _context.SystemAccounts
            .FirstOrDefaultAsync(sa => sa.UserName == username && sa.Password == password && sa.IsActive);
    }
}
