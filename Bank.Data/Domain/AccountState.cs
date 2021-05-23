using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bank.Data.Domain
{
  public enum AccountStatus
  { Requested, Active, Frozen, Dormant, Closed }

  public enum CurrencyCode
  { USD, GPB, CNY }

  [BsonIgnoreExtraElements]
  public class AccountState
  {
    public Guid AccountId { get; set; }
    public AccountStatus Status { get; set; }
    public CurrencyCode Currency { get; set; }
    public decimal Balance { get; set; }
    public decimal AllowedOverdraft { get; set; }

    public AccountState(
      Guid id,
      CurrencyCode currency = CurrencyCode.USD,
      AccountStatus status = AccountStatus.Requested,
      decimal balance = 0,
      decimal allowedOverdraft = 0)
    {
      AccountId = id;
      Currency = currency;
      Status = status;
      Balance = balance;
      AllowedOverdraft = allowedOverdraft;
    }

    public static AccountState New(Guid id) => new(id);

    public AccountState Debit(decimal? amount) => Credit(-amount);

    public AccountState Credit(decimal? amount) => new(
      id: AccountId,
      currency: Currency,
      status: Status,
      balance: Balance + amount ?? default,
      allowedOverdraft: AllowedOverdraft
    );

    public AccountState WithStatus(AccountStatus newStatus) => new(
      id: AccountId,
      currency: Currency,
      status: newStatus,
      balance: Balance,
      allowedOverdraft: AllowedOverdraft
    );
  }
}
