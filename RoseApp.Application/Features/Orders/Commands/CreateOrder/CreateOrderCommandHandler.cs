using MediatR;
using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Application.Contracts;
using RoseApp.Application.DTOs.Orders;
using RoseApp.Application.Exceptions;
using RoseApp.Domain.Entities.Commerce;
using RoseApp.Domain.Entities.UserDomain;
using RoseApp.Domain.Enums;

namespace RoseApp.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, ResponseViewModel<OrderDto>>
{
    private readonly IDbContext _context;

    public CreateOrderCommandHandler(IDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel<OrderDto>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var cart = await _context.Set<RoseApp.Domain.Entities.Commerce.Cart>()
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null)
            return ResponseViewModel<OrderDto>.Fail("Cart is empty", 400);

        var cartItems = await _context.Set<CartItem>()
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cart.Id)
            .ToListAsync(cancellationToken);

        if (!cartItems.Any())
            return ResponseViewModel<OrderDto>.Fail("Cart is empty", 400);

        var address = await _context.Set<Address>()
            .FirstOrDefaultAsync(a => a.Id == request.AddressId
                && a.UserId == request.UserId, cancellationToken);

        if (address is null)
            throw new NotFoundException(nameof(Address), request.AddressId);

        decimal totalPrice = 0;
        var orderItems = new List<OrderItem>();

        foreach (var cartItem in cartItems)
        {
            if (cartItem.Product.Stock < cartItem.Quantity)
                return ResponseViewModel<OrderDto>.Fail(
                    $"Product '{cartItem.Product.Name}' has only {cartItem.Product.Stock} items in stock", 400);

            var itemTotal = cartItem.UnitPrice * cartItem.Quantity;
            totalPrice += itemTotal;

            orderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                TotalPrice = itemTotal
            });
        }

        decimal discountAmount = 0;
        Coupon? coupon = null;

        if (!string.IsNullOrWhiteSpace(request.CouponCode))
        {
            coupon = await _context.Set<Coupon>()
                .FirstOrDefaultAsync(c => c.Code == request.CouponCode
                    && c.IsActive, cancellationToken);

            if (coupon is null)
                return ResponseViewModel<OrderDto>.Fail("Invalid or expired coupon", 400);

            if (coupon.ExpiresAt.HasValue && coupon.ExpiresAt < DateTime.UtcNow)
                return ResponseViewModel<OrderDto>.Fail("Coupon has expired", 400);

            if (coupon.MaxUses.HasValue && coupon.UsedCount >= coupon.MaxUses)
                return ResponseViewModel<OrderDto>.Fail("Coupon has reached its usage limit", 400);

            if (coupon.MinOrderAmount.HasValue && totalPrice < coupon.MinOrderAmount)
                return ResponseViewModel<OrderDto>.Fail(
                    $"Minimum order amount for this coupon is {coupon.MinOrderAmount}", 400);

            discountAmount = coupon.DiscountType == Domain.Enums.DiscountType.Percentage
                ? totalPrice * (coupon.DiscountValue / 100)
                : coupon.DiscountValue;

            discountAmount = Math.Min(discountAmount, totalPrice);

            coupon.UsedCount++;
        }

        var finalPrice = totalPrice - discountAmount;

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            AddressId = request.AddressId,
            Status = OrderStatus.Pending,
            TotalPrice = totalPrice,
            DiscountAmount = discountAmount,
            FinalPrice = finalPrice,
            CouponId = coupon?.Id,
            Notes = request.Notes,
            Items = orderItems
        };

        await _context.Set<Order>().AddAsync(order, cancellationToken);

        foreach (var cartItem in cartItems)
        {
            cartItem.Product.Stock -= cartItem.Quantity;
            _context.Entry(cartItem.Product).State = EntityState.Modified;
        }

        _context.Set<CartItem>().RemoveRange(cartItems);

        await _context.SaveChangesAsync(cancellationToken);

        var orderDto = new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            TotalPrice = order.TotalPrice,
            DiscountAmount = order.DiscountAmount,
            FinalPrice = order.FinalPrice,
            Notes = order.Notes,
            CreatedAt = order.CreatedAt,
            Address = new AddressInfoDto
            {
                Label = address.Label,
                Street = address.Street,
                City = address.City,
                Country = address.Country
            },
            Coupon = coupon == null ? null : new CouponInfoDto
            {
                Code = coupon.Code,
                DiscountAmount = discountAmount
            },
            Items = orderItems.Select(i => new OrderItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = cartItems.First(ci => ci.ProductId == i.ProductId).Product.Name,
                ProductPhoto = cartItems.First(ci => ci.ProductId == i.ProductId).Product.PhotoUrl,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return ResponseViewModel<OrderDto>.Success(orderDto, "Order created successfully", 201);
    }
}