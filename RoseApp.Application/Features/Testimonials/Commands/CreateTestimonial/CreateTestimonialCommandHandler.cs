using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Testimonials.Commands.CreateTestimonial;

public class CreateTestimonialCommandHandler
    : IRequestHandler<CreateTestimonialCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public CreateTestimonialCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        CreateTestimonialCommand request,
        CancellationToken cancellationToken)
    {
        var testimonial = new Testimonial
        {
            Id = Guid.NewGuid(),
            AuthorName = request.AuthorName,
            AuthorPhoto = request.AuthorPhoto,
            Content = request.Content,
            Rating = request.Rating
        };

        await _context.Set<Testimonial>().AddAsync(testimonial, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success(
            "Testimonial created successfully", "Created", 201);
    }
}