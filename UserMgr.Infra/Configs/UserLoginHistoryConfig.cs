using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserMgr.Domain.Entities;

namespace UserMgr.Infra.Configs
{
  public class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
  {
    public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
    {
      builder.ToTable("UserLoginHistories");
      builder.OwnsOne(x => x.PhoneNumber, nb => nb.Property(b => b.Number).HasMaxLength(20).IsUnicode(false));
    }
  }
}
