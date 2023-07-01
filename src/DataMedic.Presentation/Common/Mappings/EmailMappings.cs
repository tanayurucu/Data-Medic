using Mapster;

using DataMedic.Application.Emails.Commands.CreateEmail;
using DataMedic.Application.Emails.Commands.DeleteEmail;
using DataMedic.Application.Emails.Commands.UpdateEmail;
using DataMedic.Application.Emails.Models;
using DataMedic.Application.Emails.Queries.GetEmailById;
using DataMedic.Application.Emails.Queries.GetEmails;
using DataMedic.Contracts.Emails;
using DataMedic.Domain.Emails;

namespace DataMedic.Presentation.Common.Mappings;

public sealed class EmailMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Email, EmailResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Address, src => src.Address.Value)
            .Map(dest => dest.DepartmentId, src => src.DepartmentId.Value);

        config
            .NewConfig<EmailWithDepartment, EmailWithDepartmentResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Address, src => src.Address.Value)
            .Map(dest => dest.Department.Id, src => src.Department.Id.Value)
            .Map(dest => dest.Department.Name, src => src.Department.Name.Value);

        config.NewConfig<GetEmailsQueryParameters, GetEmailsQuery>();

        config.NewConfig<Guid, GetEmailByIdQuery>().Map(dest => dest.EmailId, src => src);

        config.NewConfig<CreateEmailRequest, CreateEmailCommand>();

        config
            .NewConfig<(Guid emailId, UpdateEmailRequest request), UpdateEmailCommand>()
            .Map(dest => dest.EmailId, src => src.emailId)
            .Map(dest => dest.Address, src => src.request.Address);

        config.NewConfig<Guid, DeleteEmailCommand>().Map(dest => dest.EmailId, src => src);
    }
}