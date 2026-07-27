using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Products.Commands.RestoreProduct;

public record RestoreProductCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;