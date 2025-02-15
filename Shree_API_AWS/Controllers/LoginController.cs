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
            Userdetailstable auth = await user.AuthenticateUser(
                login.UserName, login.Password);
            if (auth != null && auth.Role == "admin")
            {
                var tokens = await token.CreateToken(auth);
                return Ok(new {EmployeeId = auth.Employeeid, token = tokens.ToString()});
            }
            else if(auth != null && auth.Role != "admin")
            {
                return BadRequest("Not Authorized!!");
            }
            return BadRequest("Username or Pass Incorrect!");

        }
    }
}
