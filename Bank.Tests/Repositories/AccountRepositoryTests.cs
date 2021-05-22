using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bank.Data.Domain;
using Bank.Data.Infrastructure;
using Bank.Data.Repositories;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Bank.Tests.Repositories
{
  public class AccountRepositoryTests
  {
    private readonly Mock<IMongoCollectionWrapper<AccountState>> _stateDb;
    private readonly Mock<IMongoCollectionWrapper<AccountEvent>> _eventDb;

    private readonly IAccountRepository _repo;

    public AccountRepositoryTests()
    {
      _stateDb = new Mock<IMongoCollectionWrapper<AccountState>>();
      _eventDb = new Mock<IMongoCollectionWrapper<AccountEvent>>();

      var client = new Mock<IMongoClientProvider>();
      client
        .Setup(m => m.GetAccountStateCollection())
        .Returns(_stateDb.Object);
      client
        .Setup(m => m.GetAccountEventCollection())
        .Returns(_eventDb.Object);

      _repo = new AccountRepository(client.Object);
    }

    [Fact]
    public async Task GetAccountState()
    {
      _stateDb
        .Setup(m => m.FindAsync(
          It.IsAny<Expression<Func<AccountState, bool>>>(),
          null))
        .ReturnsAsync(AccountState.New(Guid.NewGuid()));

      var result = await _repo.GetAccountState(Guid.NewGuid()).Try();

      Assert.True(result.IsSome);
    }

    [Fact]
    public async Task UpsertAccountState()
    {
      _stateDb
        .Setup(m => m.FindOneAndUpdateAsync(
          It.IsAny<Expression<Func<AccountState, bool>>>(),
          It.IsAny<UpdateDefinition<AccountState>>(),
          It.IsAny<FindOneAndUpdateOptions<AccountState, AccountState>>(),
          default))
        .ReturnsAsync(AccountState.New(Guid.NewGuid()));

      var result = await _repo
        .UpsertAccountState(AccountState.New(Guid.NewGuid()))
        .Try();

      Assert.True(result.IsSome);
    }

    [Fact]
    public async Task AddAccountEvent()
    {
      _eventDb
        .Setup(m => m.InsertOneAsync(
          It.IsAny<AccountEvent>(),
          null,
          default))
        .Returns(Task.CompletedTask);

      var result = await _repo
        .AddAccountEvent(new AccountEvent())
        .Try();

      Assert.True(result.IsSome);
    }
  }
}
