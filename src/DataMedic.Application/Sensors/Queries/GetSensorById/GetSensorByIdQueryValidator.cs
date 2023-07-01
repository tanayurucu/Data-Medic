using FluentValidation;

namespace DataMedic.Application.Sensors.Queries.GetSensorById;

public sealed class GetSensorByIdQueryValidator : AbstractValidator<GetSensorByIdQuery>
{
    public GetSensorByIdQueryValidator() { }
}
