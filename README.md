# 🌹 Rose App API

> A modern E-Commerce REST API built with ASP.NET Core 10, following Clean Architecture and modern backend development practices.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat\&logo=dotnet\&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-512BD4?style=flat\&logo=dotnet\&logoColor=white)](https://learn.microsoft.com/aspnet/core)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=flat\&logo=microsoftsqlserver\&logoColor=white)](https://www.microsoft.com/sql-server)
[![Redis](https://img.shields.io/badge/Redis-Caching-DC382D?style=flat\&logo=redis\&logoColor=white)](https://redis.io/)

---

## 📌 Overview

**Rose App** is an E-Commerce backend API designed for a flower and gift shop platform.

The project was built from scratch to practice real-world backend concepts such as **Clean Architecture, CQRS, MediatR, JWT Authentication, Dynamic Authorization, Redis Caching, SignalR, Stripe Payments, and more.**

---

## 🏗️ Architecture

The project follows **Clean Architecture** with a clear separation of concerns:

```text
RoseApp
├── RoseApp.Domain
│   └── Entities, Enums, Abstractions
│
├── RoseApp.Application
│   └── CQRS, DTOs, Validators, Interfaces
│
├── RoseApp.Infrastructure
│   └── EF Core, Persistence, External Services, Seeding
│
└── RoseApp.API
    └── Controllers, Middleware, Authorization, SignalR
```

* **Domain** — Core business entities and abstractions.
* **Application** — Business use cases using CQRS and MediatR.
* **Infrastructure** — Database persistence and external services.
* **API** — REST endpoints, middleware, authorization, and SignalR.

---

## ⚙️ Tech Stack

* ASP.NET Core 10 Web API
* Entity Framework Core & SQL Server
* Clean Architecture
* CQRS & MediatR
* JWT Authentication
* Dynamic Roles & Permissions
* Redis Caching
* SignalR
* Stripe Payments & Webhooks
* Cloudinary
* MailKit
* FluentValidation
* Swagger / OpenAPI

---

## ✨ Main Features

### 🔐 Authentication & Authorization

* Email OTP verification
* JWT Authentication
* Password reset
* Dynamic Roles & Permissions
* Permission-based authorization

### 🛍️ E-Commerce

* Categories, SubCategories & Occasions
* Product management with search, filtering & pagination
* Soft Delete & Restore
* Shopping Cart
* Orders & Coupons
* Stripe Payments & Webhooks

### 👤 User Features

* Profile & Address management
* Wishlist
* Product Reviews
* Email change with OTP verification

### ⚡ Performance & Real-Time

* Redis caching with automatic cache invalidation
* Query projection & `AsNoTracking`
* Real-time notifications using SignalR

### 👑 Administration

* Dashboard statistics
* Audit logs
* User management
* Dynamic Roles & Permissions management

---

## 🧩 Design Patterns & Practices

* **Clean Architecture** — Separation of concerns
* **CQRS + MediatR** — Separate read and write operations
* **Repository & Unit of Work** — Data access and transaction management
* **MediatR Pipeline Behaviors** — Centralized validation
* **Global Exception Middleware** — Consistent error handling
* **Options Pattern** — Strongly typed configuration
* **Soft Delete** — Safe data removal and restoration
* **Projection** — Optimized database queries

---

## 🚀 Getting Started

### Prerequisites

* .NET 10 SDK
* SQL Server
* Redis
* Stripe Account
* Cloudinary Account

### Setup

**1. Clone the repository**

```bash
git clone https://github.com/HazemSaad1/RoseApp.git
cd RoseApp
```

**2. Configure the application**

Update `RoseApp.API/appsettings.json` with your own:

* SQL Server connection string
* JWT settings
* Email / SMTP credentials
* Stripe API keys & webhook secret
* Cloudinary credentials
* Redis connection string

> ⚠️ Never commit real secrets or API keys to source control.

**3. Apply migrations**

```bash
dotnet ef database update --project RoseApp.Infrastructure --startup-project RoseApp.API
```

**4. Run the API**

```bash
dotnet run --project RoseApp.API
```

**5. Open Swagger**

```text
https://localhost:7290/swagger
```

---

## 📖 API Documentation

The API is documented using **Swagger / OpenAPI**, allowing you to explore and test the available endpoints directly through Swagger UI.

---

[GitHub](https://github.com/HazemSaad1) · [LinkedIn](https://www.linkedin.com/)

---

⭐ If you find this project interesting, feel free to explore the repository and share your feedback!
