using UserMgr.Domain.ValueObjects;

namespace UserMgr.WebAPI.Controllers
{
  public record LoginByPhoneAndPwdRequest(PhoneNumber PhoneNumber, string Password);
}
