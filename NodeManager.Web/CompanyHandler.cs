using System;
using System.Threading.Tasks;
using System.Security.Claims;
using NodeManager.Web.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace NodeManager.Web
{
    public class CompanyHandler : AuthorizationHandler<CompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CompanyRequirement requirement)
        {
            if (context.User.HasClaim("Company", "Inpad") &&
                context.User.FindFirst("Company") != null &&
                context.User.FindFirst("Company").Value.Equals(requirement.Company))
                //if(context.User.FindFirst("Company")!=null && context.User.FindFirst("Company").Value.Equals(requirement.Company))
                    //if (context.User.FindFirst("Company").Value.Equals(requirement.Company))
                        context.Succeed(requirement);
            
            return Task.CompletedTask;
        }
    }
}
