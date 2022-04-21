using UserMgr.Domain.ValueObjects;

namespace UserMgr.WebAPI.Controllers
{
  public record AddUserRequest(PhoneNumber PhoneNumber, string Password);
}
