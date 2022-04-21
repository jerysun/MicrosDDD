using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMgr.Infra
{
  internal class DbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
  {
    public UserDbContext CreateDbContext(string[] args)
    {
      DbContextOptionsBuilder<UserDbContext> builder = new();
      builder.UseSqlServer("Server=.;Database=MicrosDDDDB;integrated security=True;Encrypt=False;MultipleActiveResultSets=True");
      return new UserDbContext(builder.Options);

    }
  }
}
