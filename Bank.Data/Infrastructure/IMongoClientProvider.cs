using Bank.Data.Domain;

namespace Bank.Data.Infrastructure
{
  public interface IMongoClientProvider
  {
    IMongoCollectionWrapper<AccountState> GetAccountStateCollection();
    IMongoCollectionWrapper<AccountEvent> GetAccountEventCollection();
  }
}
