using DataMedic.Infrastructure.Authentication.Constants;

using Microsoft.AspNetCore.Authorization;

namespace DataMedic.Infrastructure.Authentication.Policies;

public sealed class ExpertRequirement : AuthorizationHandler<ExpertRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExpertRequirement requirement)
    {
        if (Roles.ExpertRoles.Any(expertRole => context.User.Claims.Any(claim => claim.Value.Contains(expertRole))))
        {
            context.Succeed(requirement);
            context.Requirements.ToList().ForEach(context.Succeed);
        }

        return Task.CompletedTask;
    }
}