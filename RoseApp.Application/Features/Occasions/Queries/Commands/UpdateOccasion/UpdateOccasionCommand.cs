using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Occasions.Commands.UpdateOccasion;

public record UpdateOccasionCommand(
    Guid Id,
    string Name,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;