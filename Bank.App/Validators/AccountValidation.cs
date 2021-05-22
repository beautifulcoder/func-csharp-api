using Bank.App.Accounts.Commands;
using Bank.Data.Domain;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Bank.App.Validators
{
  public static partial class AccountValidation
  {
    public static Validation<ErrorMsg, Unit> AccountMustNotExist(
      this TryOptionAsync<AccountState> self) =>
      self
        .Match(
          Fail: (_) => Fail<ErrorMsg, Unit>("Unable to get account info"),
          Some: (acc) => Fail<ErrorMsg, Unit>($"Id {acc.AccountId} already exists"),
          None: () => Success<ErrorMsg, Unit>(unit)
        )
        .Result;

    public static Validation<ErrorMsg, AccountState> AccountMustExist(
      this TryOptionAsync<AccountState> self) =>
      self
        .Match(
          Fail: (_) => Fail<ErrorMsg, AccountState>("Unable to get account info"),
          Some: (acc) => Success<ErrorMsg, AccountState>(acc),
          None: () => Fail<ErrorMsg, AccountState>($"Account does not exist")
        )
        .Result;

    public static Validation<ErrorMsg, AccountTransaction> CurrencyMustBeSet(
      this AccountTransaction self) =>
      Optional(self)
        .Where(acc => acc.Currency != null)
        .ToValidation<ErrorMsg>("Currency must be set");

    public static Validation<ErrorMsg, AccountTransaction> AmountMustBeSet(
      this AccountTransaction self) =>
      Optional(self)
        .Where(acc => acc.Amount != null)
        .Where(acc => acc.Amount > 0)
        .ToValidation<ErrorMsg>("Amount must be set and be greater than zero");

    public static Validation<ErrorMsg, AccountState> HasEnoughFunds(
      this AccountState self, decimal? amount) =>
      Optional(self)
        .Where(acc => acc.Balance - amount + acc.AllowedOverdraft >= 0)
        .ToValidation<ErrorMsg>("Insufficient funds in the account for this transaction");
  }
}
