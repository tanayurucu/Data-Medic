using ErrorOr;

using MediatR;

namespace DataMedic.Application.Common.Messages;

internal interface ICommand<out TResponse> : IRequest<TResponse>
    where TResponse : IErrorOr
{ }