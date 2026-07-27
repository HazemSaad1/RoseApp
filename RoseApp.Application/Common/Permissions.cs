namespace RoseApp.Application.Common;

public static class Permissions
{
    public static class Categories
    {
        public const string View = "categories.view";
        public const string Create = "categories.create";
        public const string Update = "categories.update";
        public const string Delete = "categories.delete";
    }

    public static class SubCategories
    {
        public const string View = "subcategories.view";
        public const string Create = "subcategories.create";
        public const string Update = "subcategories.update";
        public const string Delete = "subcategories.delete";
    }

    public static class Occasions
    {
        public const string View = "occasions.view";
        public const string Create = "occasions.create";
        public const string Update = "occasions.update";
        public const string Delete = "occasions.delete";
    }

    public static class Products
    {
        public const string View = "products.view";
        public const string Create = "products.create";
        public const string Update = "products.update";
        public const string Delete = "products.delete";
    }

    public static class Orders
    {
        public const string View = "orders.view";
        public const string Update = "orders.update";
    }

    public static class Users
    {
        public const string View = "users.view";
        public const string Delete = "users.delete";
    }

    public static class Coupons
    {
        public const string View = "coupons.view";
        public const string Create = "coupons.create";
        public const string Update = "coupons.update";
        public const string Delete = "coupons.delete";
    }

    public static class Blogs
    {
        public const string View = "blogs.view";
        public const string Create = "blogs.create";
        public const string Update = "blogs.update";
        public const string Delete = "blogs.delete";
    }

    public static class Testimonials
    {
        public const string Update = "testimonials.update";
        public const string Delete = "testimonials.delete";
    }

    public static class Notifications
    {
        public const string Create = "notifications.create";
    }

    public static class Payments
    {
        public const string Refund = "payments.refund";
    }

    public static class Subscriptions
    {
        public const string View = "subscriptions.view";
        public const string Delete = "subscriptions.delete";
    }

    public static class Admin
    {
        public const string ViewStats = "admin.view_stats";
        public const string ViewAuditLog = "admin.view_audit_log";
        public const string DeleteAuditLog = "admin.delete_audit_log";
    }
    public static class Roles
    {
        public const string View = "roles.view";
        public const string Create = "roles.create";
        public const string Update = "roles.update";
        public const string Delete = "roles.delete";
        public const string AssignPermission = "roles.assign_permission";
        public const string AssignToUser = "roles.assign_to_user";
    }
}