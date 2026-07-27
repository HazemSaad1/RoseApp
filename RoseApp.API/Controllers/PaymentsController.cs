using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.API.Authorization;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Payments.Commands.ConfirmPayment;
using RoseApp.Application.Features.Payments.Commands.CreatePaymentIntent;
using RoseApp.Application.Features.Payments.Commands.HandleWebhook;
using RoseApp.Application.Features.Payments.Commands.RefundPayment;

namespace RoseApp.API.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Create Stripe payment intent for an order</summary>
    [HttpPost("create-intent")]
    [Authorize]
    public async Task<IActionResult> CreateIntent(
        [FromBody] CreatePaymentIntentCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Confirm a payment intent</summary>
    [HttpPost("confirm")]
    [Authorize]
    public async Task<IActionResult> Confirm(
        [FromBody] ConfirmPaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Refund a payment (Admin only)</summary>
    [HttpPost("refund")]
    [HasPermission(Permissions.Payments.Refund)]
    public async Task<IActionResult> Refund(
        [FromBody] RefundPaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.Code, result);
    }

    /// <summary>Stripe webhook endpoint (no auth required, uses signature verification)</summary>
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var payload = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].ToString();

        var result = await _mediator.Send(
            new HandleWebhookCommand(payload, signature));

        return StatusCode(result.Code, result);
    }
}