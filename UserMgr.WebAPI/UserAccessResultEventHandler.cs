using MediatR;
using UserMgr.Domain;
using UserMgr.Infra;

namespace UserMgr.WebAPI
{
  public class UserAccessResultEventHandler : INotificationHandler<UserAccessResultEvent>
  {
    private readonly IUserRepository _userRepository;
    private readonly UserDbContext _dbContext;

    public UserAccessResultEventHandler(IUserRepository userRepository, UserDbContext dbContext)
    {
      _userRepository = userRepository;
      _dbContext = dbContext;
    }

    public async Task Handle(UserAccessResultEvent notification, CancellationToken cancellationToken)
    {
      await _userRepository.AddNewLoginHistoryAsync(notification.PhoneNumber, $"The result of login: {notification.Result}");
      await _dbContext.SaveChangesAsync();
    }
  }
}
