using Bank.App.Validators;
using LanguageExt;
using MediatR;

namespace Bank.App.Accounts.Queries
{
  public class GetAccountById : Record<GetAccountById>,
    IRequest<Validation<ErrorMsg, TryOptionAsync<AccountViewModel>>>
  {
    public GetAccountById(string accountId)
    {
      AccountId = accountId;
    }

    public string AccountId { get; }
  }
}
