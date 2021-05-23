using System.Threading;
using System.Threading.Tasks;
using Bank.Data.Repositories;
using LanguageExt;
using MediatR;
using static Bank.App.Validators.StringValidation;
using AccountResponse = LanguageExt.Validation<
  Bank.App.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Bank.App.Accounts.AccountViewModel>>;

namespace Bank.App.Accounts.Queries
{
  public class GetAccountByIdHandler :
    IRequestHandler<GetAccountById, AccountResponse>
  {
    private readonly IAccountRepository _repo;

    public GetAccountByIdHandler(IAccountRepository repo)
    {
      _repo = repo;
    }

    public Task<AccountResponse> Handle(
      GetAccountById request,
      CancellationToken cancellationToken) =>
      IsValidGuid(request.AccountId)
        .Bind<TryOptionAsync<AccountViewModel>>(id =>
          _repo.GetAccountState(id)
            .Map(AccountViewModel.New)
        )
      .AsTask();
  }
}
