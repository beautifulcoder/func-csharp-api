using Bank.Data.Domain;
using MongoDB.Driver;

namespace Bank.Data.Infrastructure
{
  public class MongoClientProvider : IMongoClientProvider
  {
    private readonly IMongoClient _client;

    public MongoClientProvider(IMongoClient client)
    {
      _client = client;
    }

    public IMongoCollectionWrapper<AccountState> GetAccountStateCollection() =>
      new MongoCollectionWrapper<AccountState>(
        _client
          .GetDatabase("bank")
          .GetCollection<AccountState>("account_state")
      );

    public IMongoCollectionWrapper<AccountEvent> GetAccountEventCollection() =>
      new MongoCollectionWrapper<AccountEvent>(
        _client
          .GetDatabase("bank")
          .GetCollection<AccountEvent>("account_event")
      );
  }
}
