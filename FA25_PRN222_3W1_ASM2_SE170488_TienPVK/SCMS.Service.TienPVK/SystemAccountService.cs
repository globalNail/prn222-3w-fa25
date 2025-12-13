using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK;

namespace SCMS.Service.TienPVK;

public class SystemAccountService
{
    private readonly SystemAccountRepository _repository;

    public SystemAccountService() => _repository ??= new SystemAccountRepository();

    public SystemAccountService(SystemAccountRepository repository) => _repository = repository;

    public async Task<SystemAccount> LoginAsync(string username, string password) => await _repository.GetAccountAsync(username, password);
}
