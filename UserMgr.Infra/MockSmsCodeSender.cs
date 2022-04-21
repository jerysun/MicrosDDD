using UserMgr.Domain;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Infra
{
  public class MockSmsCodeSender : ISmsCodeSender
  {
    public Task SendCodeAsync(PhoneNumber phoneNumber, string code)
    {
      Console.WriteLine($"Send {phoneNumber.RegionCode}-{phoneNumber.Number} the verification code {code}");
      return Task.CompletedTask;
    }
  }
}
