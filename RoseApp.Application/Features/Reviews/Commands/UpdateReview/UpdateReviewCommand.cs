using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Reviews.Commands.UpdateReview;

public record UpdateReviewCommand(
    Guid ReviewId,
    Guid UserId,
    int Rating,
    string? Comment) : IRequest<ResponseViewModel<string>>;