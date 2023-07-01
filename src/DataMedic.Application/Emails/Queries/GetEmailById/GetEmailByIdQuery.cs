using DataMedic.Application.Common.Messages;

using ErrorOr;

using DataMedic.Domain.Emails;

namespace DataMedic.Application.Emails.Queries.GetEmailById;

public record GetEmailByIdQuery(Guid EmailId) : IQuery<ErrorOr<Email>>;
