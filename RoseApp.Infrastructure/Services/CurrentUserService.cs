using Microsoft.AspNetCore.Http;
using RoseApp.Application.Common;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace RoseApp.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(value, out var id) ? id : Guid.Empty;
            }
        }

        public string Email
            => _httpContextAccessor.HttpContext?
                .User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        public bool IsAuthenticated
            => _httpContextAccessor.HttpContext?
                .User.Identity?.IsAuthenticated ?? false;
    }
}
