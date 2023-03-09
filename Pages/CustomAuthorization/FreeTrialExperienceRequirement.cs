using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebAppMysql.Pages.CustomAuthorization
{
	public class FreeTrialExperienceRequirement : IAuthorizationRequirement

	{
		public FreeTrialExperienceRequirement(int freeTrialInMonths)
		{
			FreeTrialInMonths = freeTrialInMonths;
		}
		public int FreeTrialInMonths { get; }
	}
    public class FreeTrialExperienceRequirementHandler : AuthorizationHandler<FreeTrialExperienceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FreeTrialExperienceRequirement requirement)
        {
			if (!context.User.HasClaim(claim => claim.Type == "FreeTrialStartDate"))
			{
				return Task.CompletedTask;
			}
			var freeTrialStartDate = DateTime.Parse(context.User.FindFirst(claim => claim.Type == "FreeTrialStartDate").Value);
			var period = DateTime.Now - freeTrialStartDate;
			//check if the free-trial experience is expired
			if (period.Days < 30 * requirement.FreeTrialInMonths)
			{
				context.Succeed(requirement);
			}
            return Task.CompletedTask;
        }
    }
}

