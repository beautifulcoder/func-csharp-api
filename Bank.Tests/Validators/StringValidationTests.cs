using Xunit;
using static Bank.App.Validators.StringValidation;

namespace Bank.Tests.Validators
{
  public class StringValidationTests
  {
    [Fact]
    public void IsValidGuidEmpty() =>
      Assert.True(
        IsValidGuid(string.Empty)
          .IsFail
      );

    [Fact]
    public void IsValidGuidFail() =>
      Assert.True(
        IsValidGuid("-- invalid --")
          .IsFail
      );

    [Fact]
    public void IsValidGuidSuccess() =>
      Assert.True(
        IsValidGuid("669dceb5-107d-4701-ae9c-802d6963d081")
          .IsSuccess
      );
  }
}
