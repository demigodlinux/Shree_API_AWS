using Microsoft.AspNetCore.Mvc;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository;

namespace Shree_API_AWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUser user;
        private readonly IToken token;
        public LoginController(IUser user, IToken token)
        {
            this.user = user;
            this.token = token;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var auth = await user.AuthenticateUser(
                login.UserName, login.Password);
            if (auth != null)
            {
                var tokens = await token.CreateToken(auth);
                return Ok(tokens.ToString());
            }
            return BadRequest("Username or Pass Incorrect!");

        }
    }
}
