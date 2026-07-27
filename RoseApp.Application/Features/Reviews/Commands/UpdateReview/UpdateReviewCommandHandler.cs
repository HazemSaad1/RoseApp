using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler
    : IRequestHandler<UpdateReviewCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateReviewCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.Set<Review>()
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

        if (review is null)
            throw new NotFoundException(nameof(Review), request.ReviewId);

        if (review.UserId != request.UserId)
            return ResponseViewModel<string>.Fail("Unauthorized", 403);

        review.Rating = request.Rating;
        review.Comment = request.Comment;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Review updated successfully");
    }
}