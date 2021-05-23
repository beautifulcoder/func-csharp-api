using LanguageExt;
using System.Globalization;
using Xunit;
using static LanguageExt.Prelude;

namespace Bank.Tests
{
  public class Basics
  {
    [Fact]
    public void Bind() =>
      Assert.Equal(
        5,
        Optional(1)
          .Bind<int>(o => o + 2)
          .Bind<int>(o => o + 2)
          .Bind<int>(o => o)
      );

    [Fact]
    public void Map() =>
      Assert.Equal(
        "1",
        Optional(1)
          .Map(o => o.ToString(CultureInfo.InvariantCulture))
      );

    [Fact]
    public void BindProp() =>
      Assert.Equal(
        None,
        Optional(1)
          .Bind<int>(o => o + 2)
          .Bind<int>(_ => None)
          .Bind<int>(_ => 1)
      );

    [Fact]
    public void ApplyValidationFail() =>
      Assert.Equal(
        "[[fail_1], [fail_2]]",
        (
          Success<Seq<string>, Option<int>>(3),
          Fail<Seq<string>, Option<int>>(Seq1("fail_1")),
          Success<Seq<string>, Option<int>>(3), Fail<Seq<string>,
          Option<int>>(Seq1("fail_2"))
        )
        .Apply((a, b, c, d) =>
          from w in a
          from x in b
          from y in c
          from z in d
          select w + x + y + z)
        .FailToSeq()
        .ToString()
      );

    [Fact]
    public void ApplyValidationNone() =>
      Assert.Equal(
        None,
        (
          Success<Seq<string>, Option<int>>(3),
          Fail<Seq<string>, Option<int>>(Seq1("fail_1")),
          Success<Seq<string>, Option<int>>(3),
          Fail<Seq<string>, Option<int>>(Seq1("fail_2"))
        )
        .Apply((a, b, c, d) =>
          from w in a
          from x in b
          from y in c
          from z in d
          select w + x + y + z)
        .Match(
          Succ: o => o,
          Fail: _ => None
        )
      );

    [Fact]
    public void ApplyValidationSome() =>
      Assert.Equal(
        Optional(8),
        (
          Success<Seq<string>, Option<int>>(3),
          Success<Seq<string>, Option<int>>(1),
          Success<Seq<string>, Option<int>>(3),
          Success<Seq<string>, Option<int>>(1)
        )
        .Apply((a, b, c, d) =>
          from w in a
          from x in b
          from y in c
          from z in d
          select w + x + y + z)
        .Match(
          Succ: o => o,
          Fail: _ => None
        )
      );
  }
}
