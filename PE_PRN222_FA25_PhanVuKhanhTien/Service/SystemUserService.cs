using Entity.Models;
using Repository;

namespace Service;

public class SystemUserService
{
    private readonly SystemUserRepository _repository;

    public SystemUserService() => _repository = new SystemUserRepository();

    public SystemUserService(SystemUserRepository repository) => _repository = repository;

    public async Task<List<SystemUser>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<SystemUser> LoginAsync(string username, string password) => await _repository.GetAccountAsync(username, password);
}
