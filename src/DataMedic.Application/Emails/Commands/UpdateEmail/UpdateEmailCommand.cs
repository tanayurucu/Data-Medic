using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Emails.Commands.UpdateEmail;

public sealed record UpdateEmailCommand(Guid EmailId, string Address) : ICommand<ErrorOr<Updated>>;
