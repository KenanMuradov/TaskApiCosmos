using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskApiCosmos.Data;
using TaskApiCosmos.Services.Interfaces;

namespace TaskApiCosmos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStorageManager _storageManager;
        private readonly AppDbContext _context;

        public ProfileController(IUserService userService, IStorageManager storageManager, AppDbContext context)
        {
            _userService = userService;
            _storageManager = storageManager;
            _context = context;
        }

        [HttpGet("profilePhoto")]
        public async Task<ActionResult<bool>> GetPPAsync(string email)
        {
            var user = await _userService.FindUserByEmailAsync(email);
            if (user is null)
                return false;

            var url = _storageManager.GetSignedUrl(user.ProfilePhoto);
            return Ok(url);
        }
        [AllowAnonymous]
        [HttpGet("get")]
        public IActionResult Get()
        {
            var users = _context.Users?.ToList();
            return Ok(users);
        }
    }
}
