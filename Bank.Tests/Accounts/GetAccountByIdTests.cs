using System;
using System.Threading.Tasks;
using Bank.App.Accounts.Queries;
using Bank.Data.Domain;
using Bank.Data.Repositories;
using Moq;
using Xunit;
using static LanguageExt.Prelude;

namespace Bank.Tests.Accounts
{
  public class GetAccountByIdTests
  {
    private readonly Mock<IAccountRepository> _repo;
    private readonly GetAccountByIdHandler _handler;

    public GetAccountByIdTests()
    {
      _repo = new Mock<IAccountRepository>();
      _handler = new GetAccountByIdHandler(_repo.Object);
    }

    [Fact]
    public async Task HandleGetAccountById()
    {
      _repo
        .Setup(m => m.GetAccountState(It.IsAny<Guid>()))
        .Returns(TryOptionAsync(Task.FromResult(
          AccountState.New(Guid.NewGuid())))
        );

      var result = await _handler.Handle(
        new GetAccountById("669dceb5-107d-4701-ae9c-802d6963d081"),
        default);

      Assert.True(result.IsSuccess);
    }
  }
}
