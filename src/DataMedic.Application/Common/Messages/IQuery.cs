using ErrorOr;

using MediatR;

namespace DataMedic.Application.Common.Messages;

internal interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : IErrorOr { }
