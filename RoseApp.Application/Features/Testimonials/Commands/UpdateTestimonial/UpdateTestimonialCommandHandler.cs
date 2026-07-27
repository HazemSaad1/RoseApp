using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Testimonials.Commands.UpdateTestimonial;

public class UpdateTestimonialCommandHandler
    : IRequestHandler<UpdateTestimonialCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public UpdateTestimonialCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        UpdateTestimonialCommand request,
        CancellationToken cancellationToken)
    {
        var testimonial = await _context.Set<Testimonial>()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (testimonial is null)
            throw new NotFoundException(nameof(Testimonial), request.Id);

        testimonial.AuthorName = request.AuthorName;
        testimonial.Content = request.Content;
        testimonial.Rating = request.Rating;

        if (request.AuthorPhoto is not null)
            testimonial.AuthorPhoto = request.AuthorPhoto;

        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Testimonial updated successfully");
    }
}