using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Warehouse.Repository;
using Warehouse.Repository.Abstractions;

namespace WebApplication.Providers
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var repo = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IEmployeeRepository)) as EmployeeRepository;
            using (repo)
            {
                if (repo != null)
                {
                    var result = repo.Login(context.UserName, context.Password);
                    var user = repo.GetUserInfo(context.UserName);
                    if (!result.Success)
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }

                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Type));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

                    context.Validated(identity);
                }
            }
        }
    }
}