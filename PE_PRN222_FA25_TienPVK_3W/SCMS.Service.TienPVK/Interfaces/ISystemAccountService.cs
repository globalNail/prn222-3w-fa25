using SCMS.Domain.TienPVK.Models;

namespace SCMS.Service.TienPVK.Interfaces;

public interface ISystemAccountService
{
    Task<SystemAccount> LoginAsync(string username, string password);
}
