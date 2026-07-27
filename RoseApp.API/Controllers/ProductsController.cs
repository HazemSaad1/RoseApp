using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Products;
using RoseApp.Application.Features.Products.Commands.CreateProduct;
using RoseApp.Application.Features.Products.Commands.DeleteProduct;
using RoseApp.Application.Features.Products.Commands.RestoreProduct;
using RoseApp.Application.Features.Products.Commands.UpdateProduct;
using RoseApp.Application.Features.Products.Queries.GetAllProducts;
using RoseApp.Application.Features.Products.Queries.GetDeletedProducts;
using RoseApp.Application.Features.Products.Queries.GetProductById;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all products (paginated)</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ProductFilterDto filter)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(filter));
        return StatusCode(result.Code, result);
    }

    /// <summary>Get product by ID</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>List soft-deleted products (Admin only)</summary>
    [HttpGet("deleted")]
    [HasPermission(Permissions.Products.Delete)]
    public async Task<IActionResult> GetDeleted(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 20)
    {
        var result = await _mediator.Send(new GetDeletedProductsQuery(page, limit));
        return StatusCode(result.Code, result);
    }

    /// <summary>Create a new product (Admin only)</summary>
    [HttpPost]
    [HasPermission(Permissions.Products.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Update product (Admin only)</summary>
    [HttpPatch("{id:guid}")]
    [HasPermission(Permissions.Products.Update)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command with { Id = id });
        return StatusCode(result.Code, result);
    }

    /// <summary>Delete product (Admin only)</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Products.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        return StatusCode(result.Code, result);
    }

    /// <summary>Restore a soft-deleted product (Admin only)</summary>
    [HttpPost("{id:guid}/restore")]
    [HasPermission(Permissions.Products.Delete)]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _mediator.Send(new RestoreProductCommand(id));
        return StatusCode(result.Code, result);
    }
}