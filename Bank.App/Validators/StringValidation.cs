using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Bank.App.Validators
{
  public static class StringValidation
  {
    public static Validation<ErrorMsg, Guid> IsValidGuid(string str) =>
      Optional(str)
        .Where(s => !string.IsNullOrEmpty(s))
        .Where(s => Guid.TryParse(str, out _))
        .ToValidation<ErrorMsg>($"Id {str} must be a valid Guid")
        .Map(s => new Guid(s));
  }
}
