using ErrorOr;

using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Departments.ValueObjects;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Commands.CreateEmail;

internal sealed class CreateEmailCommandHandler
    : ICommandHandler<CreateEmailCommand, ErrorOr<Email>>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmailCommandHandler(
        IDepartmentRepository departmentRepository,
        IUnitOfWork unitOfWork,
        IEmailRepository emailRepository
    )
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
        _emailRepository = emailRepository;
    }

    public async Task<ErrorOr<Email>> Handle(
        CreateEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var departmentId = DepartmentId.Create(request.DepartmentId);

        ErrorOr<EmailAddress> createEmailAddressResult = EmailAddress.Create(request.Address);
        if (createEmailAddressResult.IsError)
        {
            return createEmailAddressResult.Errors;
        }

        if (
            await _emailRepository.FindByAddressAndDepartmentAsync(
                createEmailAddressResult.Value,
                departmentId,
                cancellationToken
            )
            is not null
        )
        {
            return Errors.Email.AlreadyExists;
        }

        var email = Email.Create(createEmailAddressResult.Value, departmentId);
        await _emailRepository.AddAsync(email, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return email;
    }
}
