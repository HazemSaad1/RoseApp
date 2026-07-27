using MediatR;
using RoseApp.Application.Common;
using RoseApp.Application.DTOs.Payments;

namespace RoseApp.Application.Features.Payments.Commands.CreatePaymentIntent;

public record CreatePaymentIntentCommand(Guid OrderId)
    : IRequest<ResponseViewModel<PaymentIntentDto>>;