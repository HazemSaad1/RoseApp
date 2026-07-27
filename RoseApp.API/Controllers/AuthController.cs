using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Features.Auth.Commands.ConfirmEmailVerification;
using RoseApp.Application.Features.Auth.Commands.ForgotPassword;
using RoseApp.Application.Features.Auth.Commands.Login;
using RoseApp.Application.Features.Auth.Commands.Register;
using RoseApp.Application.Features.Auth.Commands.ResetPassword;
using RoseApp.Application.Features.Auth.Commands.SendEmailVerification;

namespace RoseApp.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Send OTP to email (step 1 before registration)</summary>
        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerification(
            [FromBody] SendEmailVerificationCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        /// <summary>Confirm OTP; email is then marked verified for registration</summary>
        [HttpPost("confirm-email-verification")]
        public async Task<IActionResult> ConfirmEmailVerification(
            [FromBody] ConfirmEmailVerificationCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        /// <summary>Register (email must be verified via send + confirm email first)</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        /// <summary>Sign in with email</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        /// <summary>Request password reset (sends email with reset link/code)</summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        /// <summary>Reset password using token from forgot-password</summary>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }
    }
}