using System;
using System.Threading.Tasks;
using Bank.App.Accounts;
using Bank.App.Accounts.Commands;
using Bank.App.Validators;
using Bank.Data.Domain;
using Bank.Web.Controllers;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static LanguageExt.Prelude;
using AccountResponse = LanguageExt.Validation<
  Bank.App.Validators.ErrorMsg,
  LanguageExt.TryOptionAsync<Bank.App.Accounts.AccountViewModel>>;

namespace Bank.Tests.Controllers
{
  public class AccountsControllerTests
  {
    private readonly Mock<IMediator> _mediator;
    private readonly AccountsController _controller;

    public AccountsControllerTests()
    {
      _mediator = new Mock<IMediator>();
      _controller = new AccountsController(_mediator.Object);
    }

    [Fact]
    public async Task Get()
    {
      _mediator
        .Setup(m => m.Send(
          It.IsAny<IRequest<AccountResponse>>(),
          default))
        .ReturnsAsync(
          Success<ErrorMsg, TryOptionAsync<AccountViewModel>>(
            TryOptionAsync(AccountViewModel.New(
              AccountState.New(Guid.NewGuid())))
          ));

      var result = await _controller.Get("-- id --");

      Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Post()
    {
      _mediator
        .Setup(m => m.Send(
          It.IsAny<IRequest<AccountResponse>>(),
          default))
        .ReturnsAsync(
          Success<ErrorMsg, TryOptionAsync<AccountViewModel>>(
            TryOptionAsync(AccountViewModel.New(
              AccountState.New(Guid.NewGuid())))
          ));

      var result = await _controller.Post(new AccountTransaction());

      Assert.IsType<OkObjectResult>(result);
    }
  }
}
