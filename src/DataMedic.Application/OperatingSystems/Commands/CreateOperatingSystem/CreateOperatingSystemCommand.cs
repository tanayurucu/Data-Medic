using DataMedic.Application.Common.Messages;
using DataMedic.Domain.OperatingSystems;

using ErrorOr;

using OperatingSystem = DataMedic.Domain.OperatingSystems.OperatingSystem;

namespace DataMedic.Application.OperatingSystems.Commands.CreateOperatingSystem;

public sealed record CreateOperatingSystemCommand(string Name) : ICommand<ErrorOr<OperatingSystem>>;