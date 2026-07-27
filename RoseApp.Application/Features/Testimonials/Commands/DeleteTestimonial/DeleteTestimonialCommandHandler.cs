using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Content;

namespace RoseApp.Application.Features.Testimonials.Commands.DeleteTestimonial;

public class DeleteTestimonialCommandHandler
    : IRequestHandler<DeleteTestimonialCommand, ResponseViewModel<string>>
{
    private readonly IDbContext _context;

    public DeleteTestimonialCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<string>> Handle(
        DeleteTestimonialCommand request,
        CancellationToken cancellationToken)
    {
        var testimonial = await _context.Set<Testimonial>()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (testimonial is null)
            throw new NotFoundException(nameof(Testimonial), request.Id);

        _context.Set<Testimonial>().Remove(testimonial);
        await _context.SaveChangesAsync(cancellationToken);

        return ResponseViewModel<string>.Success("Testimonial deleted successfully");
    }
}