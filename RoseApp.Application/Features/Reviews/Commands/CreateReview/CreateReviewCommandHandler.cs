using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Catalog;
using RoseApp.Domain.Entities.UserDomain;

namespace RoseApp.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateReviewCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var productExists = await _context.Set<Product>()
            .AnyAsync(p => p.Id == request.ProductId, cancellationToken);

        if (!productExists)
            throw new NotFoundException(nameof(Product), request.ProductId);

        var alreadyReviewed = await _context.Set<Review>()
            .AnyAsync(r => r.UserId == request.UserId
                && r.ProductId == request.ProductId, cancellationToken);

        if (alreadyReviewed)
            return ResponseViewModel<string>.Fail(
                "You have already reviewed this product", 409);

        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        await _context.Set<Review>().AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Review created successfully", "Created", 201);
    }
}