using DataMedic.Application.Common.Messages;

using ErrorOr;

namespace DataMedic.Application.Emails.Commands.DeleteEmail;

public sealed record DeleteEmailCommand(Guid EmailId) : ICommand<ErrorOr<Deleted>>;
