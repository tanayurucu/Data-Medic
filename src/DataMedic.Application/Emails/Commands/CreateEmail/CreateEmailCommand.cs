using DataMedic.Application.Common.Messages;

using ErrorOr;
using DataMedic.Domain.Emails;

namespace DataMedic.Application.Emails.Commands.CreateEmail;

public sealed record CreateEmailCommand(Guid DepartmentId, string Address)
    : ICommand<ErrorOr<Email>>;
