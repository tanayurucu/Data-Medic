using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Hosts.Commands.DeleteHost;

public class DeleteHostCommandValidator : AbstractValidator<DeleteHostCommand>
{
    public DeleteHostCommandValidator()
    {
        RuleFor(x => x.HostId)
            .NotEmpty()
            .WithError(Errors.Host.HostIdRequired)
            .NotEqual(Guid.Empty)
            .WithError(Errors.Host.HostIdRequired);
    }
}
