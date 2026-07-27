using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoseApp.Application.Common;
using RoseApp.Application.Features.Users.Commands.ChangePassword;
using RoseApp.Application.Features.Users.Commands.ConfirmEmailChange;
using RoseApp.Application.Features.Users.Commands.DeleteAccount;
using RoseApp.Application.Features.Users.Commands.RequestEmailChange;
using RoseApp.Application.Features.Users.Commands.UpdateProfile;
using RoseApp.Application.Features.Users.Queries.GetProfile;

namespace RoseApp.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>Get current user profile</summary>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(
                new GetProfileQuery(_currentUserService.UserId));
            return StatusCode(result.Code, result);
        }

        /// <summary>Update profile (firstName, lastName, photo, phone)</summary>
        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateProfileCommand command)
        {
            var result = await _mediator.Send(
                command with { UserId = _currentUserService.UserId });
            return StatusCode(result.Code, result);
        }

        /// <summary>Change password (authenticated)</summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(
                command with { UserId = _currentUserService.UserId });
            return StatusCode(result.Code, result);
        }

        /// <summary>Request email change (sends verification code to new email)</summary>
        [HttpPost("email/request")]
        public async Task<IActionResult> RequestEmailChange(
            [FromBody] RequestEmailChangeCommand command)
        {
            var result = await _mediator.Send(
                command with { UserId = _currentUserService.UserId });
            return StatusCode(result.Code, result);
        }

        /// <summary>Confirm email change with code</summary>
        [HttpPost("email/confirm")]
        public async Task<IActionResult> ConfirmEmailChange(
            [FromBody] ConfirmEmailChangeCommand command)
        {
            var result = await _mediator.Send(
                command with { UserId = _currentUserService.UserId });
            return StatusCode(result.Code, result);
        }

        /// <summary>Delete own account (disabled for super admin)</summary>
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await _mediator.Send(
                new DeleteAccountCommand(_currentUserService.UserId));
            return StatusCode(result.Code, result);
        }
    }
}