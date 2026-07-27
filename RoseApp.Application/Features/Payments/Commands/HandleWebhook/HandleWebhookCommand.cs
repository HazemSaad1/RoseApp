using MediatR;
using RoseApp.Application.Common;

namespace RoseApp.Application.Features.Payments.Commands.HandleWebhook;

public record HandleWebhookCommand(
    string Payload,
    string Signature) : IRequest<ResponseViewModel<string>>;