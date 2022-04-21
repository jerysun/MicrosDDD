using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;

namespace UserMgr.Infra
{
  public class UserRepository : IUserRepository
  {
    private readonly UserDbContext _userDbContext;
    private readonly IDistributedCache _distributedCache;
    private readonly IMediator _mediator;

    public UserRepository(UserDbContext userDbContext, IDistributedCache distributedCache, IMediator mediator)
    {
      _userDbContext = userDbContext;
      _distributedCache = distributedCache;
      _mediator = mediator;
    }

    public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg)
    {
      var user = await FindOneAsync(phoneNumber);
      UserLoginHistory userLoginHistory = new UserLoginHistory(user?.Id, phoneNumber, msg);

      //Not used SaveChangesAsync() yet, which should be called in controllers. Should in UserDbContext do:
      //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      //which is a cooperation with MediatR - an atomic transaction
      _userDbContext.UserLoginHistories.Add(userLoginHistory);
    }

    public async Task<User?> FindOneAsync(PhoneNumber phoneNumber) => await _userDbContext.Users
      .Include(u => u.AccessFail)
      .Where(x => x.PhoneNumber.Number == phoneNumber.Number && x.PhoneNumber.RegionCode == phoneNumber.RegionCode)
      .SingleOrDefaultAsync();

    public async Task<User?> FindOneAsync(Guid userId) => await _userDbContext.Users
      .Where(x => x.Id == userId)
      .SingleOrDefaultAsync();

    public Task PublishEventAsync(UserAccessResultEvent eventData)
    {
      return _mediator.Publish(eventData);
    }

    // fetch the so-called "verification code"
    public Task<string> RetrievePhoneCodeAsync(PhoneNumber phoneNumber)
    {
      string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
      string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
      string? code = _distributedCache.GetString(cacheKey);
      //The verification code is one-time, it won't be used any more
      _distributedCache.Remove(cacheKey);
      return Task.FromResult(code);
    }

    // save the "verification code"
    // If there's only one await statement and it's the last sentence,
    // then we shouldn't use async/await keywords for the purpose of
    // optimization. But please add the keyword return in front of the
    // last statement
    public /*async*/ Task SavePhoneCodeAsync(PhoneNumber phoneNumber, string code)
    {
      string fullNumber = phoneNumber.RegionCode + phoneNumber.Number;
      string cacheKey = $"LoginByPhoneAndCode_Code_{fullNumber}";
      /*await*/ return _distributedCache.SetStringAsync(cacheKey, code, new DistributedCacheEntryOptions
      {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
      });
    }
  }
}
