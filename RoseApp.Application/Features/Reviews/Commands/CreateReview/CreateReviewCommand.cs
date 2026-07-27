using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    Guid UserId,
    Guid ProductId,
    int Rating,
    string? Comment) : IRequest<ResponseViewModel<string>>;