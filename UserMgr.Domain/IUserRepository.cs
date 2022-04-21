using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain
{
  public interface IUserRepository
  {
    Task<User?> FindOneAsync(PhoneNumber phoneNumber);
    Task<User?> FindOneAsync(Guid userId);
    Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg);
    Task PublishEventAsync(UserAccessResultEvent eventData);
    Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code);
    Task<string> RetrievePhoneCodeAsync(PhoneNumber phoneNumber);
  }
}
