using System.Threading.Tasks;
using Bank.App.Accounts.Queries;
using Bank.App.Accounts.Commands;
using Bank.Web.Extensions;
using LanguageExt;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountsController : ControllerBase
  {
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    public Task<IActionResult> Get(string id) =>
      _mediator
        .Send(new GetAccountById(id))
        .Bind(acc => acc.ToActionResult());

    [HttpPost]
    public Task<IActionResult> Post([FromBody] AccountTransaction trans) =>
      _mediator
        .Send(trans)
        .Bind(acc => acc.ToActionResult());
  }
}
