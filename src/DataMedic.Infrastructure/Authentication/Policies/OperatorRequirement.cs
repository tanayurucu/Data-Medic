using DataMedic.Infrastructure.Authentication.Constants;

using Microsoft.AspNetCore.Authorization;

namespace DataMedic.Infrastructure.Authentication.Policies;

public sealed class OperatorRequirement : AuthorizationHandler<OperatorRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperatorRequirement requirement)
    {
        if (Roles.OperatorRoles.Any(
                operatorRole => context.User.Claims.Any(claim => claim.Value.Contains(operatorRole))))
        {
            context.Succeed(requirement);
            context.Requirements.ToList().ForEach(context.Succeed);
        }

        return Task.CompletedTask;
    }
}