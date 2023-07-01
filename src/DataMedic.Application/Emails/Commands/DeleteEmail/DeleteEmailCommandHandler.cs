using ErrorOr;

using DataMedic.Application.Common.Interfaces.Persistence;
using DataMedic.Application.Common.Interfaces.Persistence.Repositories;
using DataMedic.Application.Common.Messages;
using DataMedic.Domain.Common.Errors;
using DataMedic.Domain.Emails;
using DataMedic.Domain.Emails.ValueObjects;

namespace DataMedic.Application.Emails.Commands.DeleteEmail;

internal sealed class DeleteEmailCommandHandler
    : ICommandHandler<DeleteEmailCommand, ErrorOr<Deleted>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailRepository _emailRepository;

    public DeleteEmailCommandHandler(IUnitOfWork unitOfWork, IEmailRepository emailRepository)
    {
        _unitOfWork = unitOfWork;
        _emailRepository = emailRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(
        DeleteEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var emailId = EmailId.Create(request.EmailId);
        if (await _emailRepository.FindByIdAsync(emailId, cancellationToken) is not Email email)
        {
            return Errors.Email.NotFound;
        }

        _emailRepository.Remove(email);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
