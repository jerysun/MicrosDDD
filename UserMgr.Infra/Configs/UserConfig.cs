using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserMgr.Domain.Entities;

namespace UserMgr.Infra.Configs
{
  public class UserConfig : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("Users");
      builder.OwnsOne(x => x.PhoneNumber, nb =>
      {
        nb.Property(b => b.Number).HasMaxLength(20).IsUnicode(false);
      });
      builder.Property("passwordHash").HasMaxLength(100).IsUnicode(false);
      builder.HasOne(x => x.AccessFail).WithOne(x => x.User)
        .HasForeignKey<UserAccessFail>(x => x.UserId);
    }
  }
}