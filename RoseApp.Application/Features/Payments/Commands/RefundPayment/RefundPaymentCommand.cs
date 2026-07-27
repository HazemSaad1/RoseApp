using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Payments.Commands.RefundPayment;

public record RefundPaymentCommand(string PaymentIntentId)
    : IRequest<ResponseViewModel<string>>;