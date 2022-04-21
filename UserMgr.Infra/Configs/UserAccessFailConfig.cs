using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserMgr.Domain.Entities;

namespace UserMgr.Infra.Configs
{
  public class UserAccessFailConfig : IEntityTypeConfiguration<UserAccessFail>
  {
    public void Configure(EntityTypeBuilder<UserAccessFail> builder)
    {
      builder.ToTable("UserAccessFails");
      builder.Property("lockedOut");
    }
  }
}
