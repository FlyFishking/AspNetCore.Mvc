using System.Threading.Tasks;
using WebApplication4.Middleware.Platform;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class UserSessionStore : IUserSessionStore
    {
        public Task<AppUser> GetAsync(string key) {
            return Task.FromResult(new AppUser());
        }

        public Task RemoveAsync(string key) {
            return Task.FromResult(0);
        }

        public Task RenewAsync(string key, AppUser user) {
            return Task.FromResult(0);
        }
    }
}
