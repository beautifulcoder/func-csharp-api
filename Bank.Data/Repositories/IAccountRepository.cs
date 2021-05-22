using System;
using Bank.Data.Domain;
using LanguageExt;

namespace Bank.Data.Repositories
{
  public interface IAccountRepository
  {
    TryOptionAsync<AccountState> GetAccountState(Guid id);
    TryOptionAsync<AccountState> UpsertAccountState(AccountState state);
    TryOptionAsync<Unit> AddAccountEvent(AccountEvent accountEvent);
  }
}
