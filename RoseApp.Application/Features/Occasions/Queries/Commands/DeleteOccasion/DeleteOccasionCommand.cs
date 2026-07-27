using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Occasions.Commands.DeleteOccasion;

public record DeleteOccasionCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;