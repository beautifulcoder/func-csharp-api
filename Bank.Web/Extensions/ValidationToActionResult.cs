using System.Threading.Tasks;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Bank.App.Validators;

namespace Bank.Web.Extensions
{
  public static class ValidationToActionResult
  {
    public static Task<IActionResult> ToActionResult<T>(
      this Validation<ErrorMsg, TryOptionAsync<T>> self) =>
      self.Match(
        Fail: e => Task.FromResult<IActionResult>(
          new BadRequestObjectResult(e)),
        Succ: valid => valid.Match<T, IActionResult>(
          Some: r => new OkObjectResult(r),
          None: () => new NotFoundResult(),
          Fail: _ => new StatusCodeResult(
            StatusCodes.Status500InternalServerError)
        )
      );
  }
}
