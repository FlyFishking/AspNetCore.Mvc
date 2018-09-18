using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace QuickstartIdentityServer
{
    /// <summary>
    /// 因为要使用登录的时候要使用数据中保存的用户进行验证，要实IResourceOwnerPasswordValidator接口
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var pwd = context.Password;
            var checkUserC = userName == "Bob" && pwd == "password";
            var userId = 1;
            if (checkUserC)
            {
                var claims = GetUserClaims();
//                context.Result = new GrantValidationResult(userName, "custom", claims);
                context.Result = new GrantValidationResult(userId.ToString(), OidcConstants.AuthenticationMethods.Password, claims);
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid credential");
            }
        }

        private IEnumerable<Claim> GetUserClaims() => new Claim[]
        {
            new Claim("UserId", 1.ToString()),
            new Claim(JwtClaimTypes.Name,"Bob"),
            new Claim(JwtClaimTypes.GivenName, "Tom jekson"),
            new Claim(JwtClaimTypes.FamilyName, "yyy"),
            new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
            new Claim(JwtClaimTypes.Role,"admin")
        };
    }
}