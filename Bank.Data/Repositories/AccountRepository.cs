using System;
using Bank.Data.Domain;
using Bank.Data.Infrastructure;
using LanguageExt;
using MongoDB.Driver;
using static LanguageExt.Prelude;

namespace Bank.Data.Repositories
{
  public class AccountRepository : IAccountRepository
  {
    private readonly IMongoCollectionWrapper<AccountState> _stateDb;
    private readonly IMongoCollectionWrapper<AccountEvent> _eventDb;

    public AccountRepository(IMongoClientProvider client)
    {
      _stateDb = client.GetAccountStateCollection();
      _eventDb = client.GetAccountEventCollection();
    }

    public TryOptionAsync<AccountState> GetAccountState(Guid id) =>
      TryOptionAsyncExtensions.RetryBackOff(
        TryOptionAsync(() => _stateDb.FindAsync(a => a.AccountId == id)),
        100);

    public TryOptionAsync<AccountState> UpsertAccountState(AccountState state) =>
      TryOptionAsync(() => _stateDb.FindOneAndUpdateAsync(
        acc => acc.AccountId == state.AccountId,
        Builders<AccountState>.Update
          .Set(acc => acc.AccountId, state.AccountId)
          .Set(acc => acc.Status, state.Status)
          .Set(acc => acc.Currency, state.Currency)
          .Set(acc => acc.Balance, state.Balance)
          .Set(acc => acc.AllowedOverdraft, state.AllowedOverdraft),
        new FindOneAndUpdateOptions<AccountState, AccountState>
        {
          IsUpsert = true,
          ReturnDocument = ReturnDocument.After
        }
      ));

    public TryOptionAsync<Unit> AddAccountEvent(AccountEvent accountEvent) =>
      TryOptionAsyncExtensions.RetryBackOff(
        TryOptionAsync(() => _eventDb.InsertOneAsync(accountEvent).ToUnit()),
        100,
        5);
  }
}
