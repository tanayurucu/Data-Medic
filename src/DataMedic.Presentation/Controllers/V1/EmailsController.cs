using MapsterMapper;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using DataMedic.Application.Emails.Commands.CreateEmail;
using DataMedic.Application.Emails.Commands.DeleteEmail;
using DataMedic.Application.Emails.Commands.UpdateEmail;
using DataMedic.Application.Emails.Queries.GetEmailById;
using DataMedic.Application.Emails.Queries.GetEmails;
using DataMedic.Contracts.Common;
using DataMedic.Contracts.Emails;
using DataMedic.Contracts.Routes;
using DataMedic.Infrastructure.Authentication.Constants;
using DataMedic.Presentation.Controllers.Base;

namespace DataMedic.Presentation.Controllers.V1;

[ApiVersion("1.0")]
public sealed class EmailsController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public EmailsController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet(ApiRoutes.Emails.Get)]
    public async Task<IActionResult> Get(
        [FromQuery] GetEmailsQueryParameters queryParameters,
        CancellationToken cancellationToken
    )
    {
        var query = _mapper.Map<GetEmailsQuery>(queryParameters);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<PagedResponse<EmailWithDepartmentResponse>>(value)),
            errors => Problem(errors)
        );
    }

    [HttpGet(ApiRoutes.Emails.GetById)]
    public async Task<IActionResult> GetById(Guid emailId, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetEmailByIdQuery>(emailId);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(
            value => Ok(_mapper.Map<EmailResponse>(value)),
            errors => Problem(errors)
        );
    }

    [HttpPost(ApiRoutes.Emails.Create)]
    public async Task<IActionResult> Create(
        CreateEmailRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<CreateEmailCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(
            value =>
                CreatedAtAction(
                    nameof(GetById),
                    new { EmailId = value.Id.Value },
                    _mapper.Map<EmailResponse>(value)
                ),
            errors => Problem(errors)
        );
    }

    [HttpPut(ApiRoutes.Emails.Update)]
    public async Task<IActionResult> Update(
        Guid emailId,
        UpdateEmailRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = _mapper.Map<UpdateEmailCommand>((emailId, request));

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), errors => Problem(errors));
    }

    [HttpDelete(ApiRoutes.Emails.Delete)]
    public async Task<IActionResult> Delete(Guid emailId, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<DeleteEmailCommand>(emailId);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(_ => NoContent(), errors => Problem(errors));
    }
}
