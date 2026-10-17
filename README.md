# Rose App API

A RESTful e-commerce backend for a flower and gift shop, built with **ASP.NET Core 10** using **Clean Architecture** and **CQRS**.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-EF%20Core-CC2927?logo=microsoftsqlserver&logoColor=white)](https://learn.microsoft.com/ef/core)
[![Stripe](https://img.shields.io/badge/Stripe-Payments-635BFF?logo=stripe&logoColor=white)](https://stripe.com/)

---

## Overview

Rose App covers the full flow of an online store — catalog, cart, checkout, and payments — together with an admin side for managing content, orders, users, and access control.

Every use case is implemented as a MediatR command or query with its own handler and validator, which keeps business logic isolated and easy to extend.

## Architecture

```text
RoseApp.slnx
├── RoseApp.Domain           Entities, enums, base abstractions (no dependencies)
├── RoseApp.Application      CQRS features (commands, queries, handlers, validators),
│                            DTOs, mapping profiles, contracts, pipeline behaviors
├── RoseApp.Infrastructure   EF Core DbContext, configurations, migrations, seeding,
│                            JWT, email, Cloudinary, Stripe, audit-log services
└── RoseApp.API              Controllers, permission-based authorization,
                             global exception middleware, SignalR hub, Swagger
```

Dependencies point inward: **API → Infrastructure → Application → Domain**.

Handlers depend on an `IDbContext` abstraction defined in the Application layer, so business logic stays independent of the concrete EF Core implementation.

## Features

**Authentication & Users**
- Registration with email OTP verification
- JWT authentication with BCrypt password hashing
- Forgot / reset password flow
- Profile management, password change, and email change with OTP confirmation
- Address book

**Authorization**
- Dynamic roles and permissions stored in the database
- Custom `[HasPermission]` attribute backed by a dynamic authorization policy provider
- `SuperAdmin` role seeded automatically with all permissions

**Catalog**
- Categories, sub-categories, and occasions
- Products with search, filtering (category, occasion, price range), sorting, and pagination
- Soft delete and restore for products
- Product reviews

**Shopping & Checkout**
- Shopping cart and wishlist
- Orders with status management
- Coupons with percentage or fixed-amount discounts
- Stripe payment intents, confirmation, refunds, and webhooks

**Content**
- Blogs (with slug lookup), testimonials, and newsletter subscriptions
- Image upload via Cloudinary

**Administration & Real-Time**
- Dashboard statistics
- Admin audit logs
- Real-time user notifications with SignalR

## Tech Stack

| Category | Technologies |
| --- | --- |
| Framework | ASP.NET Core 10 Web API |
| Data Access | Entity Framework Core 10, SQL Server |
| Application | MediatR (CQRS), FluentValidation, AutoMapper |
| Security | JWT Bearer, BCrypt, permission-based authorization |
| Integrations | Stripe, Cloudinary, MailKit (SMTP) |
| Real-Time | SignalR |
| Documentation | Swagger / OpenAPI |

## Design Patterns & Practices

- **Clean Architecture** with strict inward dependencies
- **CQRS with MediatR** — one handler per use case
- **Validation pipeline behavior** — FluentValidation runs automatically before every handler
- **Global exception middleware** for consistent error responses
- **Soft delete** with restore support
- **Read-optimized queries** using projection and `AsNoTracking`
- **Automatic migrations and seeding** on startup

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or Developer edition)
- Accounts for [Stripe](https://stripe.com/), [Cloudinary](https://cloudinary.com/), and an SMTP provider (e.g. a Gmail app password)

### 1. Clone the repository

```bash
git clone https://github.com/HazemSaad1/RoseApp.git
cd RoseApp
```

### 2. Configure settings

Create `RoseApp.API/appsettings.Development.json` (it is git-ignored) and fill in your values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RoseAppDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "<at-least-32-characters-secret-key>"
  },
  "EmailSettings": {
    "SenderEmail": "<your-email>",
    "Password": "<your-app-password>"
  },
  "StripeSettings": {
    "SecretKey": "<sk_test_...>",
    "PublishableKey": "<pk_test_...>",
    "WebhookSecret": "<whsec_...>"
  },
  "CloudinarySettings": {
    "CloudName": "<cloud-name>",
    "ApiKey": "<api-key>",
    "ApiSecret": "<api-secret>"
  }
}
```

### 3. Run the API

```bash
dotnet run --project RoseApp.API
```

The database is created, migrated, and seeded automatically on first run.

### 4. Explore the API

Open Swagger UI at **https://localhost:7290/swagger**.

To call protected endpoints, log in via `POST /api/auth/login`, click **Authorize**, and paste the returned token.

## API Overview

| Module | Base Route | Description |
| --- | --- | --- |
| Auth | `/api/auth` | Register, login, email verification, password reset |
| Users | `/api/users` | Profile, password and email change, account deletion |
| Addresses | `/api/addresses` | User address book |
| Categories | `/api/categories`, `/api/sub-categories` | Catalog structure |
| Occasions | `/api/occasions` | Occasion-based browsing |
| Products | `/api/products` | Filtering, pagination, soft delete & restore |
| Reviews | `/api/reviews` | Product reviews |
| Cart | `/api/cart` | Shopping cart |
| Wishlist | `/api/wishlist` | Saved products |
| Orders | `/api/orders` | Order placement and status updates |
| Coupons | `/api/coupons` | Discount coupons |
| Payments | `/api/payments` | Payment intents, confirmation, refunds, webhook |
| Roles & Permissions | `/api/roles`, `/api/permissions` | Dynamic access control |
| Admin | `/api/admin` | Statistics and audit logs |
| Notifications | `/api/notifications`, `/notifications` (hub) | Stored and real-time notifications |
| Content | `/api/blogs`, `/api/testimonials`, `/api/subscriptions` | Site content and newsletter |
| Upload | `/api/upload` | Image upload |
| Health | `/health` | Health check |

## Author

**Hazem Saad** — [GitHub](https://github.com/HazemSaad1) · [LinkedIn](https://www.linkedin.com/in/hazem-saad1/)
