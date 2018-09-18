using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace QuickstartIdentityServer
{

    /// <summary>
    ///  context.Subject.Claims就是之前实现IResourceOwnerPasswordValidator接口时
    /// claims: GetUserClaims()给到的数据。
    /// </summary>
    public class ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                if (context.IssuedClaims.Count == 0)
                {
                    if (context.Subject.Claims.Any())
                    {
                        //depending on the scope accessing the user data.
                        var claims = context.Subject.Claims.ToList();
                        //set issued claims to return
                        context.IssuedClaims = claims;
                    }
                }

            }
            catch (Exception ex)
            {
                //log your error
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}