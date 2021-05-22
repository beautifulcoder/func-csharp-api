using System;
using System.Threading.Tasks;
using Bank.App.Accounts.Commands;
using Bank.App.Validators;
using Bank.Data.Domain;
using MongoDB.Driver;
using Xunit;
using static LanguageExt.Prelude;

namespace Bank.Tests.Validators
{
  public class AccountValidationTests
  {
    [Fact]
    public void AccountMustNotExistFail() =>
      Assert.True(
        TryOptionAsync<AccountState>(new MongoException("fail"))
          .AccountMustNotExist()
          .IsFail
      );

    [Fact]
    public void AccountMustNotExistSome() =>
      Assert.True(
        TryOptionAsync(() => Task.FromResult(
            AccountState.New(Guid.NewGuid())))
          .AccountMustNotExist()
          .IsFail
      );

    [Fact]
    public void AccountMustNotExistNone() =>
      Assert.True(
        TryOptionAsync<AccountState>(None)
          .AccountMustNotExist()
          .IsSuccess
      );

    [Fact]
    public void AccountMustExistFail() =>
      Assert.True(
        TryOptionAsync<AccountState>(new MongoException("fail"))
          .AccountMustExist()
          .IsFail
      );

    [Fact]
    public void AccountMustExistSome() =>
      Assert.True(
        TryOptionAsync(() => Task.FromResult(
            AccountState.New(Guid.NewGuid())))
          .AccountMustExist()
          .IsSuccess
      );

    [Fact]
    public void AccountMustExistNone() =>
      Assert.True(
        TryOptionAsync<AccountState>(None)
          .AccountMustExist()
          .IsFail
      );

    [Fact]
    public void CurrencyMustBeSetFail() =>
      Assert.True(
        new AccountTransaction()
          .CurrencyMustBeSet()
          .IsFail
      );

    [Fact]
    public void CurrencyMustBeSetSuccess() =>
      Assert.True(
        new AccountTransaction
        { Currency = CurrencyCode.USD }
          .CurrencyMustBeSet()
          .IsSuccess
      );

    [Fact]
    public void AmountMustBeSetFail() =>
      Assert.True(
        new AccountTransaction()
          .AmountMustBeSet()
          .IsFail
      );

    [Fact]
    public void AmountBelowZeroFail() =>
      Assert.True(
        new AccountTransaction
        { Amount = -1 }
          .AmountMustBeSet()
          .IsFail
      );

    [Fact]
    public void AmountMustBeSetSuccess() =>
      Assert.True(
        new AccountTransaction
        { Amount = 1 }
          .AmountMustBeSet()
          .IsSuccess
      );

    [Fact]
    public void HasEnoughFundsFail() =>
      Assert.True(
        new AccountState(
          Guid.NewGuid())
          .HasEnoughFunds(9)
          .IsFail
      );

    [Fact]
    public void HasEnoughFundsSuccess() =>
      Assert.True(
        new AccountState(
          Guid.NewGuid(),
          balance: 9)
          .HasEnoughFunds(9)
          .IsSuccess
      );
  }
}
