using Microsoft.EntityFrameworkCore;
using Shree_API_AWS.Context;
using Shree_API_AWS.Models;
using Shree_API_AWS.Repository;

namespace Shree_API_AWS.Services
{
    public class UserService : IUser
    {
        private readonly ShreeDbContext_Postgres _context;

        public UserService(ShreeDbContext_Postgres context) {
            _context = context;
        }

        public async Task<Userdetailstable> AuthenticateUser(string username, string password)
        {
            List<Userdetailstable> userDetails = await _context.Userdetailstables.ToListAsync();
            var users = userDetails.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.Password.Equals(password));
            return users;
        }
    }
}
