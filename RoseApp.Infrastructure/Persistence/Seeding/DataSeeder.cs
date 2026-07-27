using Microsoft.EntityFrameworkCore;
using RoseApp.Application.Common;
using RoseApp.Domain.Entities.Identity;

namespace RoseApp.Infrastructure.Persistence.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await SeedPermissionsAsync(context);
        await SeedSuperAdminAsync(context);
    }

    private static async Task SeedPermissionsAsync(AppDbContext context)
    {
        var existingPermissions = await context.Permissions
            .Select(p => p.Name)
            .ToListAsync();

        var allPermissionNames = typeof(Permissions)
            .GetNestedTypes()
            .SelectMany(t => t.GetFields())
            .Select(f => f.GetValue(null)?.ToString())
            .Where(v => v is not null)
            .Cast<string>()
            .ToList();

        var newPermissions = allPermissionNames
            .Where(name => !existingPermissions.Contains(name))
            .Select(name => new Permission
            {
                Id = Guid.NewGuid(),
                Name = name
            })
            .ToList();

        if (newPermissions.Any())
            await context.Permissions.AddRangeAsync(newPermissions);

        if (!await context.Roles.AnyAsync(r => r.Name == "SuperAdmin"))
        {
            var superAdminRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = "SuperAdmin",
                Description = "Super Administrator with full access"
            };

            await context.Roles.AddAsync(superAdminRole);
            await context.SaveChangesAsync();

            var allPermissions = await context.Permissions.ToListAsync();

            var rolePermissions = allPermissions.Select(p => new RolePermission
            {
                RoleId = superAdminRole.Id,
                PermissionId = p.Id
            });

            await context.Set<RolePermission>().AddRangeAsync(rolePermissions);
        }
        else
        {
            var superAdminRole = await context.Roles
                .FirstAsync(r => r.Name == "SuperAdmin");

            var existingRolePermissions = await context.Set<RolePermission>()
                .Where(rp => rp.RoleId == superAdminRole.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            var newRolePermissions = newPermissions
                .Select(p => new RolePermission
                {
                    RoleId = superAdminRole.Id,
                    PermissionId = p.Id
                });

            await context.Set<RolePermission>().AddRangeAsync(newRolePermissions);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedSuperAdminAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Email == "superadmin@roseapp.com"))
            return;

        var superAdminRole = await context.Roles
            .FirstOrDefaultAsync(r => r.Name == "SuperAdmin");

        if (superAdminRole is null) return;

        var superAdminUser = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Super",
            LastName = "Admin",
            UserName = "superadmin",
            Email = "superadmin@roseapp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin@1234"),
            IsEmailVerified = true
        };

        await context.Users.AddAsync(superAdminUser);

        await context.Set<UserRole>().AddAsync(new UserRole
        {
            UserId = superAdminUser.Id,
            RoleId = superAdminRole.Id
        });

        await context.SaveChangesAsync();
    }
}