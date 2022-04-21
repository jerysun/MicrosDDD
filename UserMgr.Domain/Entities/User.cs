using UserMgr.Domain.ValueObjects;

namespace UserMgr.Domain.Entities
{
  public record User : IAggregateRoot
  {
    public Guid Id { get; init; }
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    private string? passwordHash;
    public UserAccessFail AccessFail { get; private set; } = default!;
    
    private User() { }//used for EF Core to load data

    public User(PhoneNumber phoneNumber)
    {
      Id = Guid.NewGuid();
      PhoneNumber = phoneNumber;
      AccessFail = new UserAccessFail(this);
    }

    public bool HasPassword() => !string.IsNullOrEmpty(passwordHash);

    public void ChangePassword(string value)
    {
      if (value.Length < 3) throw new ArgumentException("The password must be at least 3 characters!");
      passwordHash = HashHelper.ComputeMd5Hash(value);
    }

    public bool CheckPassword(string password) => passwordHash == HashHelper.ComputeMd5Hash(password);

    public void ChangePhoneNumber(PhoneNumber phoneNumber) => PhoneNumber = phoneNumber;
  }
}
