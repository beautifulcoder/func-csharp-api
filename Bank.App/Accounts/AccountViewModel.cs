using System;
using Bank.Data.Domain;
using LanguageExt;

namespace Bank.App.Accounts
{
  public class AccountViewModel : Record<AccountViewModel>
  {
    public Guid AccountId { get; }
    public AccountStatus Status { get; }
    public CurrencyCode Currency { get; }
    public decimal Balance { get; }
    public decimal AllowedOverdraft { get; }

    public AccountViewModel(AccountState state)
    {
      AccountId = state.AccountId;
      Currency = state.Currency;
      Status = state.Status;
      Balance = state.Balance;
      AllowedOverdraft = state.AllowedOverdraft;
    }

    public static AccountViewModel New(AccountState state) =>
      new(state);
  }
}
