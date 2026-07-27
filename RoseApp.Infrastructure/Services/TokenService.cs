using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RoseApp.Application.Contracts;
using RoseApp.Application.Contracts.IServices;
using RoseApp.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RoseApp.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IDbContext _context;

    public TokenService(IConfiguration configuration, IDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> GenerateTokenAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var keyString = jwtSettings["Key"]!;
        var keyBytes = Encoding.UTF8.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var userRoles = await _context.Set<UserRole>()
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync(cancellationToken);

        var roleIds = userRoles.Select(ur => ur.RoleId).ToList();

        var roles = await _context.Set<Role>()
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        var rolePermissions = await _context.Set<RolePermission>()
            .Where(rp => roleIds.Contains(rp.RoleId))
            .ToListAsync(cancellationToken);

        var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

        var permissions = await _context.Set<Permission>()
            .Where(p => permissionIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role.Name));

        foreach (var permission in permissions)
            claims.Add(new Claim("permission", permission.Name));

        var durationInDays = double.Parse(jwtSettings["DurationInDays"]!);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(durationInDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}