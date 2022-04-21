using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
  public interface ISmsCodeSender
  {
    Task SendCodeAsync(PhoneNumber phoneNumber, string code);
  }
}
