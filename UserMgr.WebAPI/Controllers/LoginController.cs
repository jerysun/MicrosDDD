using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgr.Domain;
using UserMgr.Infra;

namespace UserMgr.WebAPI.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class LoginController : ControllerBase
  {
    private readonly UserDomainService _userDomainService;

    public LoginController(UserDomainService userDomainService)
    {
      _userDomainService = userDomainService;
    }

    [UnitOfWork(typeof(UserDbContext))]
    [HttpPost]
    public async Task<IActionResult> LoginByPhoneAndPwd(LoginByPhoneAndPwdRequest req)
    {
      if (req.Password.Length < 3) return BadRequest("The length of password cannot be shorter than 3!");

      var phoneNum = req.PhoneNumber;
      var result = await _userDomainService.CheckLoginAsync(phoneNum, req.Password);
      switch(result)
      {
        case UserAccessResult.OK:
          return Ok("Login is successful.");
        case UserAccessResult.PhoneNumberNotFound:
          return BadRequest("Phone number or password error!");//can't be 404 to confuse the hackers
        case UserAccessResult.Lockout:
          return BadRequest("You're locked out, please try it later!");
        case UserAccessResult.NoPassword:
        case UserAccessResult.PasswordError:
          return BadRequest("Phone number or password error!");
        default:
          throw new NotImplementedException();
      }
    }
  }
}
