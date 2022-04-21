using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
  public class UserDomainService
  {
    private IUserRepository _userRepository;
    private ISmsCodeSender _smsSender;

    public UserDomainService(IUserRepository userRepository, ISmsCodeSender smsSender)
    {
      _userRepository = userRepository;
      _smsSender = smsSender;
    }

    public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNumber, string password)
    {
      // Count how many times "user" appears here? It proves everything is around the Aggregate root: "user"
      User? user = await _userRepository.FindOneAsync(phoneNumber);
      UserAccessResult result;

      if (user == null)
      {
        result = UserAccessResult.PhoneNumberNotFound;
      } else if(IsLockedOut(user))
      {
        result = UserAccessResult.Lockout;
      } else if(!user.HasPassword())
      {
        result = UserAccessResult.NoPassword;
      } else if (user.CheckPassword(password))
      {
        result = UserAccessResult.OK;
      } else
      {
        result = UserAccessResult.PasswordError;
      }

      if (user != null)
      {
        if (result == UserAccessResult.OK) ResetAccessFail(user);
        else AccessFail(user);
      }

      UserAccessResultEvent eventItem = new(phoneNumber, result);
      await _userRepository.PublishEventAsync(eventItem);
      return result;
    }

    public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNumber, string code)
    {
      var user = await _userRepository.FindOneAsync(phoneNumber);
      if (user == null) return CheckCodeResult.PhoneNumberNotFound;
      if (IsLockedOut(user)) return CheckCodeResult.Lockout;

      string? codeInServer = await _userRepository.RetrievePhoneCodeAsync(phoneNumber);
      if (string.IsNullOrEmpty(codeInServer)) return CheckCodeResult.CodeError;

      if (codeInServer == code) return CheckCodeResult.OK;
      else
      {
        AccessFail(user);
        return CheckCodeResult.CodeError;
      }
    }

    public void ResetAccessFail(User user) => user.AccessFail.Reset();

    public bool IsLockedOut(User user) => user.AccessFail.IsLockedOut();

    public void AccessFail(User user) => user.AccessFail.Fail();
  }
}
