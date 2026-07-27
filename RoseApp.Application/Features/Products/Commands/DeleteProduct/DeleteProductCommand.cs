using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id)
    : IRequest<ResponseViewModel<string>>;