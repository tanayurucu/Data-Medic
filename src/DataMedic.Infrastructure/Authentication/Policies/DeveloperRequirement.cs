using DataMedic.Infrastructure.Authentication.Constants;

using Microsoft.AspNetCore.Authorization;

namespace DataMedic.Infrastructure.Authentication.Policies;

public sealed class DeveloperRequirement : AuthorizationHandler<DeveloperRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DeveloperRequirement requirement)
    {
        if (context.User.Claims.Any(claim => claim.Value.Contains(Roles.DeveloperRole)))
        {
            context.Succeed(requirement);
            context.Requirements.ToList().ForEach(context.Succeed);
        }

        return Task.CompletedTask;
    }
}