using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using UserMgr.Infra;

namespace UserMgr.WebAPI
{
  public class UnitOfWorkFilter : IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var result = await next(); // Execute Action method
      //Only when action method is executed successfully, SaveChangesAsync() will be finally called
      if (result.Exception != null) return;

      var actionDesc = context.ActionDescriptor as ControllerActionDescriptor;
      if (actionDesc == null) return;

      var uowAttr = actionDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
      if (uowAttr == null) return;

      foreach (var dbCtxType in uowAttr.DbContextTypes)
      {
        //Get the instance from DI
        var dbCtx = context.HttpContext.RequestServices.GetService(dbCtxType) as UserDbContext;
        if (dbCtx == null) continue;
        await dbCtx.SaveChangesAsync();
      }
    }
  }
}
