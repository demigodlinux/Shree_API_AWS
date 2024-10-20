using Shree_API_AWS.Models;

namespace Shree_API_AWS.Repository
{
    public interface IToken
    {
      Task<string> CreateToken(UserDetailsTable user);
    }
}
