using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Payments.Commands.ConfirmPayment;

public record ConfirmPaymentCommand(string PaymentIntentId)
    : IRequest<ResponseViewModel<string>>;