using Shree_API_AWS.Models;

namespace Shree_API_AWS.Repository
{
    public interface IUser
    {
        public Task<UserDetailsTable> AuthenticateUser(string username, string password);
    }
}
