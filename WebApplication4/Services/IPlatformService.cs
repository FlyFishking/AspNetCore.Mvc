using System.Security.Claims;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    //平台接口
    public interface IPlatformService
    {

        bool CheckValid(ClaimsPrincipal ticket);

        AppUser GetPlatformUser(string userName);

        void SignOut(string userName);
    }
}
