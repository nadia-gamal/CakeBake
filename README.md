# 🍰 SugarBloom

> A full-stack online bakery store built with ASP.NET Core MVC.

SugarBloom is a web-based bakery e-commerce application that allows customers to browse bakery products, manage their shopping cart, place orders, and manage their profiles.

The project also includes an admin dashboard for managing products, categories, orders, customers, and store-related content.

---

## ✨ Features

### 👩‍💻 Customer Features

- User Registration & Login
- Email verification and OTP-based password reset
- Browse bakery products
- Search and filter products
- View product details
- Add products to cart
- Update cart quantities
- Checkout and place orders
- View order history
- View order details
- Manage personal profile
- Contact the store

### 🔐 Admin Features

- Admin authentication
- Admin dashboard
- Product management
- Category management
- Order management
- Customer management
- Admin profile management
- Contact message management
- Store settings management

---

## 🛠️ Technologies Used

### Backend

- C#
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity
- LINQ

### Database

- SQL Server
- Entity Framework Core Migrations

### Frontend

- HTML5
- CSS3
- JavaScript
- Bootstrap
- jQuery
- Razor Views

### Services & Tools

- Cloudinary for image management
- Email service for verification and password reset
- Visual Studio
- Git & GitHub

---

## 🏗️ Project Architecture

The project follows a structured MVC architecture with a service layer.

```text
SugarBloom
│
├── Configurations
├── Constants
├── Controllers
├── Data
├── Enums
├── Interfaces
├── Migrations
├── Models
├── Seed
├── Services
├── ViewModels
├── Views
└── wwwroot
```

### Main Layers

#### Controllers

- Handle HTTP requests
- Manage application flow
- Communicate with services

#### Services

- Contain business logic
- Handle database operations
- Keep controllers clean and maintainable

#### Models

- Represent application entities and database tables

#### ViewModels

- Transfer the required data between controllers and views

#### Views

- Razor-based UI pages for customers and administrators

---

## 📸 Screenshots

### 🏠 Home Page

![Home Page](Screenshots/home.png)

### 🛍️ Shop

![Shop](Screenshots/shop.png)

### 🍰 Product Details

![Product Details](Screenshots/product-details.png)

### 🛒 Shopping Cart

![Shopping Cart](Screenshots/cart.png)

### 💳 Checkout

![Checkout](Screenshots/checkout.png)

### ⚙️ Admin Dashboard

![Admin Dashboard](Screenshots/admin-dashboard.png)

### 📦 Product Management

![Product Management](Screenshots/products.png)

### 📋 Orders

![Orders](Screenshots/orders.png)

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/nadia-gamal/CakeBake.git
```

### 2. Open the project

Open the solution:

```text
SugarBloom.sln
```

using Visual Studio.

### 3. Configure the database

Update the connection string in:

```text
appsettings.json
```

according to your SQL Server configuration.

### 4. Apply migrations

Open Package Manager Console and run:

```powershell
Update-Database
```

### 5. Run the application

Run the project from Visual Studio or use:

```bash
dotnet run
```

---

## 🔑 Authentication

The application uses ASP.NET Core Identity for authentication and authorization.

Different user roles are supported, including:

- Customer
- Admin

Authentication-related functionality includes:

- Registration
- Login
- Email verification
- OTP password reset
- Role-based authorization

---

## 🗄️ Main Entities

The application includes several entities such as:

- ApplicationUser
- Product
- Category
- Cart
- CartItem
- Order
- OrderItem
- ContactMessage
- OtpCode

---

## 🎯 Project Goals

The main goal of SugarBloom is to build a complete bakery e-commerce experience while applying practical software development concepts such as:

- MVC architecture
- Service layer architecture
- Entity Framework Core
- Authentication & Authorization
- Database relationships
- CRUD operations
- ViewModels
- Dependency Injection
- Image management
- Order and cart management

---

## 👩‍💻 Author

**Nadia Gamal**

GitHub:  
https://github.com/nadia-gamal