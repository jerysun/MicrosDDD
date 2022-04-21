using MediatR;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
  public record UserAccessResultEvent(PhoneNumber PhoneNumber, UserAccessResult Result) : INotification;
}
