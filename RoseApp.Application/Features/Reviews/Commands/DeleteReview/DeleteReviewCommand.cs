using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand(
    Guid ReviewId,
    Guid UserId) : IRequest<ResponseViewModel<string>>;