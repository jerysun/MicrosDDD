using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMgr.Domain;
using UserMgr.Domain.Entities;
using UserMgr.Domain.ValueObjects;
using UserMgr.Infra;

namespace UserMgr.WebAPI.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class CRUDController : ControllerBase
  {
    private readonly UserDbContext _userDbContext;
    private readonly IUserRepository _userRepository;

    public CRUDController(UserDbContext userDbContext, IUserRepository userRepository)
    {
      _userDbContext = userDbContext;
      _userRepository = userRepository;
    }

    [UnitOfWork(typeof(UserDbContext))]
    [HttpPost]
    public async Task<IActionResult> AddNew(AddUserRequest req)
    {
      if (await _userRepository.FindOneAsync(req.PhoneNumber) != null)
      {
        return BadRequest("The phone number is already existed!");
      }

      User user = new User(req.PhoneNumber);
      user.ChangePassword(req.Password);
      _userDbContext.Add(user);

      return Ok("The new user has been added successfully.");
    }
  }
}
