using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Middleware.Platform
{
    public interface IUserSessionStore
    {
        Task RenewAsync(string key, AppUser user);

        Task<AppUser> GetAsync(string key);

        Task RemoveAsync(string key);
    }
}
