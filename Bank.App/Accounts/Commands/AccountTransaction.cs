using System;
using Bank.App.Validators;
using Bank.Data.Domain;
using LanguageExt;
using MediatR;

namespace Bank.App.Accounts.Commands
{
  public enum TransactionEvent
  { CreatedAccount, ActivatedAccount, AlteredOverdraft, FrozeAccount, DebitedFee, DepositedCash }

  public class AccountTransaction : Record<AccountTransaction>,
    IRequest<Validation<ErrorMsg, TryOptionAsync<AccountViewModel>>>
  {
    public TransactionEvent? Event { get; set; }
    public string AccountId { get; set; }
    public CurrencyCode? Currency { get; set; }
    public decimal? Amount { get; set; }

    public CreatedAccount ToCreatedEvent() => new()
    {
      EntityId = new Guid(AccountId),
      Currency = Currency.Value
    };

    public DepositedCash ToDepositedEvent() => new()
    {
      EntityId = new Guid(AccountId),
      Amount = Amount.Value
    };

    public DebitedFee ToDebitedEvent() => new()
    {
      EntityId = new Guid(AccountId),
      Amount = Amount.Value
    };
  }
}
