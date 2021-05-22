using System.Threading;
using System.Threading.Tasks;
using Bank.App.Validators;
using Bank.Data.Domain;
using Bank.Data.Repositories;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;
using static Bank.App.Validators.StringValidation;
using AccountResponse = LanguageExt.Validation<
  Bank.App.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Bank.App.Accounts.AccountViewModel>>;

namespace Bank.App.Accounts.Commands
{
  public class AccountTransactionHandler :
    IRequestHandler<AccountTransaction, AccountResponse>
  {
    private readonly IAccountRepository _repo;

    public AccountTransactionHandler(IAccountRepository repo)
    {
      _repo = repo;
    }

    public Task<AccountResponse> Handle(
      AccountTransaction request,
      CancellationToken cancellationToken) =>
      request.Event switch
      {
        TransactionEvent.CreatedAccount => CreatedAccount(request),
        TransactionEvent.DebitedFee => DebitedFee(request),
        TransactionEvent.DepositedCash => DepositedCash(request),
        _ => InvalidTransactionEvent()
      };

    private Task<AccountResponse> CreatedAccount(AccountTransaction request) =>
      (IsValidGuid(request.AccountId), request.CurrencyMustBeSet())
        .Apply((id, trans) =>
          _repo.GetAccountState(id).AccountMustNotExist()
            .Map((_) => PersistAccountInfo(
              AccountState.New(id), trans.ToCreatedEvent())))
        .Bind(resp => resp)
        .AsTask();

    private Task<AccountResponse> DebitedFee(AccountTransaction request) =>
      (IsValidGuid(request.AccountId), request.AmountMustBeSet())
        .Apply((id, trans) =>
          _repo.GetAccountState(id).AccountMustExist()
            .Bind(acc => acc.HasEnoughFunds(trans.Amount))
            .Bind<TryOptionAsync<AccountViewModel>>(acc =>
              PersistAccountInfo(
                acc.Debit(trans.Amount), trans.ToDebitedEvent())))
        .Bind(resp => resp)
        .AsTask();

    private Task<AccountResponse> DepositedCash(AccountTransaction request) =>
      (IsValidGuid(request.AccountId), request.AmountMustBeSet())
        .Apply((id, trans) =>
          _repo.GetAccountState(id).AccountMustExist()
            .Bind<TryOptionAsync<AccountViewModel>>(acc =>
              PersistAccountInfo(
                acc.Credit(trans.Amount), trans.ToDepositedEvent())))
        .Bind(resp => resp)
        .AsTask();

    private TryOptionAsync<AccountViewModel> PersistAccountInfo(
      AccountState state,
      AccountEvent accountEvent) =>
      from st in _repo.UpsertAccountState(state)
      from _ in _repo.AddAccountEvent(accountEvent)
      select AccountViewModel.New(st);

    private static Task<AccountResponse> InvalidTransactionEvent() =>
      Fail<ErrorMsg, TryOptionAsync<AccountViewModel>>(
        ErrorMsg.New("Invalid transaction event"))
      .AsTask();
  }
}
