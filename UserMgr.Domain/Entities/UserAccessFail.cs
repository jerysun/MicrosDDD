namespace UserMgr.Domain.Entities
{
  public record UserAccessFail
  {
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public User? User { get; init; }
    private bool lockedOut;
    public DateTime? LockOutEnd { get; private set; }
    public int AccessFailedCount { get; private set; }

    private UserAccessFail() { }

    public UserAccessFail(User user)
    {
      Id = Guid.NewGuid();
      User = user;
    }

    public void Reset()
    {
      lockedOut = false;
      LockOutEnd = null;
      AccessFailedCount = 0;
    }

    public void Fail() // process a login failure
    {
      ++AccessFailedCount;
      if (AccessFailedCount >= 3)
      {
        lockedOut = true;
        LockOutEnd = DateTime.Now.AddMinutes(5);
      }
    }

    public bool IsLockedOut()
    {
      if (lockedOut)
      {
        if (LockOutEnd >= DateTime.Now)
        {
          return true;
        }
        else
        {
          Reset();
          return false;
        }
      } else
      {
        return false;
      }
    }
  }
}
