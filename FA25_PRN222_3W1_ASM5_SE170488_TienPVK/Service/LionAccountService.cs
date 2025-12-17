using Entity.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LionAccountService
    {
        private readonly LionAccountRepository _repository;

        public LionAccountService() => _repository ??= new LionAccountRepository();

        public LionAccountService(LionAccountRepository repository) => _repository = repository;

        public async Task<LionAccount> LoginAsync(string username, string password) => await _repository.GetAccountAsync(username, password);
    }
}
