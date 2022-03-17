using Microsoft.AspNetCore.Authorization;

namespace NodeManager.Web.Requirements
{
    public class CompanyRequirement : IAuthorizationRequirement
    {
        protected internal string Company { get; set; }

        public CompanyRequirement(string companyName)
        {
            Company = companyName;
        }
    }
}
