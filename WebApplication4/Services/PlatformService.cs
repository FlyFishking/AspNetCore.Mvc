using System.Security.Claims;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class PlatformService : IPlatformService
    {
        public bool CheckValid(ClaimsPrincipal ticket) {
            return true;
        }

        public AppUser GetPlatformUser(string userName) {
            return new AppUser();
        }

        public void SignOut(string userName) {
            
        }
    }
}
