using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository;

namespace Shree_API_AWS.Services
{
    public class UserService : IUser
    {
        private readonly MasterContext _context;

        public UserService(MasterContext context) {
            _context = context;
        }

        public async Task<UserDetailsTable> AuthenticateUser(string username, string password)
        {
            List<UserDetailsTable> userDetails = await _context.UserDetailsTables.ToListAsync();
            var users = userDetails.Find(x => x.UserName.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password.Equals(password));
            return users;
        }
    }
}
