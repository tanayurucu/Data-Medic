using ErrorOr;

using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Commands.UpdateEmail;

internal sealed class UpdateEmailCommandHandler
    : ICommandHandler<UpdateEmailCommand, ErrorOr<Updated>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailRepository _emailRepository;

    public UpdateEmailCommandHandler(IUnitOfWork unitOfWork, IEmailRepository emailRepository)
    {
        _unitOfWork = unitOfWork;
        _emailRepository = emailRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdateEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var emailId = EmailId.Create(request.EmailId);
        if (await _emailRepository.FindByIdAsync(emailId, cancellationToken) is not Email email)
        {
            return Errors.Email.NotFound;
        }

        ErrorOr<EmailAddress> createEmailAddressResult = EmailAddress.Create(request.Address);
        if (createEmailAddressResult.IsError)
        {
            return createEmailAddressResult.Errors;
        }

        if (
            await _emailRepository.FindByAddressAndDepartmentAsync(
                createEmailAddressResult.Value,
                email.DepartmentId,
                cancellationToken
            )
                is Email existingEmail
            && existingEmail.Id != email.Id
        )
        {
            return Errors.Email.AlreadyExists;
        }

        email.SetAddress(createEmailAddressResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
