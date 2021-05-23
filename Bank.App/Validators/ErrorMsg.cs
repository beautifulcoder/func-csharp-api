using LanguageExt;

namespace Bank.App.Validators
{
  public class ErrorMsg : NewType<ErrorMsg, string>
  {
    public ErrorMsg(string str) : base(str) { }
    public string Title => Value;
    public static implicit operator ErrorMsg(string str) => New(str);
  }
}
