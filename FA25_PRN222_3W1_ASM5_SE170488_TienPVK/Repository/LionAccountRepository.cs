using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DBContext;

namespace Repository;

public class LionAccountRepository : GenericRepository<LionAccount>
{
    public LionAccountRepository()
    {
    }

    public LionAccountRepository(SU25LionDbContext context) => _context = context;

    public async Task<LionAccount> GetAccountAsync(string username, string password)
    {
        var result = await _context.LionAccounts
            .FirstOrDefaultAsync(predicate: sa => sa.Email == username && sa.Password == password);

        if (result is not null)
        {
            return result;
        }

        return null!;
    }
}