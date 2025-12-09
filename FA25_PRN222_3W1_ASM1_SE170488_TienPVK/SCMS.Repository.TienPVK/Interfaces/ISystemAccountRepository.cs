using SCMS.Domain.TienPVK.Models;

namespace SCMS.Repository.TienPVK.Interfaces;

public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
{
    Task<SystemAccount> GetAccountAsync(string username, string password);
}
