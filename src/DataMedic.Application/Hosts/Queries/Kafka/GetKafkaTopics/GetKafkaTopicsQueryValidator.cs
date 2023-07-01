using DataMedic.Application.Common.Extensions;
using DataMedic.Domain.Common.Errors;

using FluentValidation;

namespace DataMedic.Application.Hosts.Queries.Kafka.GetKafkaTopics;

public sealed class GetKafkaTopicsQueryValidator : AbstractValidator<GetKafkaTopicsQuery>
{
    public GetKafkaTopicsQueryValidator()
    {
        RuleFor(x => x.HostId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithError(Errors.Host.HostIdRequired);
    }
}
