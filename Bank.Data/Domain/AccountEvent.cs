using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Bank.Data.Domain
{
  [BsonIgnoreExtraElements]
  public class AccountEvent
  {
    public Guid EntityId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string EventName => GetType().ToString();
  }

  public class CreatedAccount : AccountEvent
  {
    public CurrencyCode Currency { get; set; }
  }

  public class AlteredOverdraft : AccountEvent
  {
    public decimal By { get; set; }
  }

  public class FrozeAccount : AccountEvent { }

  public class DepositedCash : AccountEvent
  {
    public decimal Amount { get; set; }
  }

  public class DebitedFee : AccountEvent
  {
    public decimal Amount { get; set; }
  }
}
