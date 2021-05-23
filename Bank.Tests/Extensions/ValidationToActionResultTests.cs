using System;
using System.Threading.Tasks;
using Bank.App.Accounts;
using Bank.App.Validators;
using Bank.Data.Domain;
using Bank.Web.Extensions;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Xunit;
using static LanguageExt.Prelude;

namespace Bank.Tests.Extensions
{
  public class ValidationToActionResultTests
  {
    [Fact]
    public async Task ToActionResultBadRequest() =>
      Assert.NotNull(
        await Fail<ErrorMsg, TryOptionAsync<AccountViewModel>>("fail")
          .ToActionResult()
        as BadRequestObjectResult
      );

    [Fact]
    public async Task ToActionResultOk() =>
      Assert.NotNull(
        await Success<ErrorMsg, TryOptionAsync<AccountViewModel>>(
            TryOptionAsync(AccountViewModel.New(
              AccountState.New(Guid.NewGuid()))))
          .ToActionResult()
        as OkObjectResult
      );

    [Fact]
    public async Task ToActionResultNotFound() =>
      Assert.NotNull(
        (await Success<ErrorMsg, TryOptionAsync<AccountViewModel>>(
            TryOptionAsync<AccountViewModel>(None))
          .ToActionResult())
        as NotFoundResult
      );

    [Fact]
    public async Task ToActionResultServerError() =>
      Assert.NotNull(
        await Success<ErrorMsg, TryOptionAsync<AccountViewModel>>(
              TryOptionAsync<AccountViewModel>(new MongoException("fail")))
            .ToActionResult()
        as StatusCodeResult
      );
  }
}
