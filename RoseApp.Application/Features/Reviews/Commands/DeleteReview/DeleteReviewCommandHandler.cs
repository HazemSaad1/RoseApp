using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteReviewCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.Set<Review>()
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

        if (review is null)
            throw new NotFoundException(nameof(Review), request.ReviewId);

        if (review.UserId != request.UserId)
            return ResponseViewModel<string>.Fail("Unauthorized", 403);

        _context.Set<Review>().Remove(review);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Review deleted successfully");
    }
}