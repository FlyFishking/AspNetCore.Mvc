using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APi
{
    //密码模式（resource owner password credentials）
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        [HttpGet]
                [TestAuthorize("User_Edit1")]
//        [Authorize]
        public async Task<IActionResult> Get()
        {
//            await CallIdentityServer.MainAsync();
//            return new JsonResult("Hello Word");
            var data = from c in HttpContext.User.Claims select new { c.Type, c.Value };
            return new JsonResult(data);
//            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

//        [HttpGet]
//        public async Task<IActionResult> Logout(string logoutId)
//        {
//            await HttpContext.SignOutAsync();
//            //            var logout = await _interaction.GetLogoutContextAsync(logoutId);
//            //            if (logout.PostLogoutRedirectUri != null)
//            //            {
//            //                return Redirect(logout.PostLogoutRedirectUri);
//            //            }
//            var refererUrl = Request.Headers["Referer"].ToString();
//            return Redirect(refererUrl);
//        }
    }
}
