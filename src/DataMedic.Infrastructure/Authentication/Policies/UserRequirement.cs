using DataMedic.Infrastructure.Authentication.Constants;

using Microsoft.AspNetCore.Authorization;

namespace DataMedic.Infrastructure.Authentication.Policies;

public sealed class UserRequirement : AuthorizationHandler<UserRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        if (Roles.UserRoles.Any(userRole => context.User.Claims.Any(claim => claim.Value.Contains(userRole))))
        {
            context.Succeed(requirement);
            context.Requirements.ToList().ForEach(context.Succeed);
        }

        return Task.CompletedTask;
    }
}