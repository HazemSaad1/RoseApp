using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Occasions.Commands.CreateOccasion;

public record CreateOccasionCommand(
    string Name,
    string? PhotoUrl) : IRequest<ResponseViewModel<string>>;