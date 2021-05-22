using System;
using System.Threading.Tasks;
using Bank.App.Accounts.Commands;
using Bank.Data.Domain;
using Bank.Data.Repositories;
using Moq;
using Xunit;
using static LanguageExt.Prelude;

namespace Bank.Tests.Accounts
{
  public class AccountTransactionTests
  {
    private readonly Mock<IAccountRepository> _repo;
    private readonly AccountTransactionHandler _handler;

    public AccountTransactionTests()
    {
      _repo = new Mock<IAccountRepository>();
      _handler = new AccountTransactionHandler(_repo.Object);
    }

    [Fact]
    public async Task HandleCreatedAccount()
    {
      _repo
        .Setup(m => m.GetAccountState(It.IsAny<Guid>()))
        .Returns(TryOptionAsync<AccountState>(None));
      _repo
        .Setup(m => m.UpsertAccountState(It.IsAny<AccountState>()))
        .Returns(TryOptionAsync(Task.FromResult(
          AccountState.New(Guid.NewGuid())))
        );

      var result = await _handler.Handle(new AccountTransaction
      {
        Event = TransactionEvent.CreatedAccount,
        AccountId = "669dceb5-107d-4701-ae9c-802d6963d081",
        Currency = CurrencyCode.USD
      }, default);

      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleDebitedFee()
    {
      _repo
        .Setup(m => m.GetAccountState(It.IsAny<Guid>()))
        .Returns(TryOptionAsync(Task.FromResult(
          new AccountState(Guid.NewGuid(), balance: 10)))
        );
      _repo
        .Setup(m => m.UpsertAccountState(It.IsAny<AccountState>()))
        .Returns(TryOptionAsync(Task.FromResult(
          AccountState.New(Guid.NewGuid())))
        );

      var result = await _handler.Handle(new AccountTransaction
      {
        Event = TransactionEvent.DebitedFee,
        AccountId = "669dceb5-107d-4701-ae9c-802d6963d081",
        Amount = 10
      }, default);

      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleDepositedCash()
    {
      _repo
        .Setup(m => m.GetAccountState(It.IsAny<Guid>()))
        .Returns(TryOptionAsync(Task.FromResult(
          AccountState.New(Guid.NewGuid())))
        );
      _repo
        .Setup(m => m.UpsertAccountState(It.IsAny<AccountState>()))
        .Returns(TryOptionAsync(Task.FromResult(
          AccountState.New(Guid.NewGuid())))
        );

      var result = await _handler.Handle(new AccountTransaction
      {
        Event = TransactionEvent.DepositedCash,
        AccountId = "669dceb5-107d-4701-ae9c-802d6963d081",
        Amount = 10
      }, default);

      Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleInvalidTransaction() =>
      Assert.True(
        (await _handler.Handle(new AccountTransaction
        {
          Event = (TransactionEvent)999
        }, default))
        .IsFail
      );
  }
}
