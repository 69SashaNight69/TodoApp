# To-Do Web Application

A full-stack, enterprise-grade To-Do web application developed as a showcase project. It features a modern Angular frontend connected to a robust .NET Core REST API with a Microsoft SQL Server database.

## Key Features

* **Secure Authentication:** User registration and login using JWT (JSON Web Tokens) with passwords securely hashed using BCrypt.
* **Task Management:** Full CRUD operations for tasks (create, read, update, delete).
* **Category Organization:** Users can create custom categories to group tasks.
* **Pagination, Search, and Filtering:** Real-time search across tasks, pagination for performance, and filtering tasks by categories.
* **Race Condition Protection:** Database-level unique composite index `(UserId, Name)` to prevent duplicate category inserts during high-concurrency requests.
* **Centralized Error Handling:** Global Exception Middleware on the backend to elegantly map business errors to correct HTTP responses.

---

## Technology Stack

### Backend
* **Platform:** .NET Core Web API (using Clean Architecture principles)
* **Database Access:** Entity Framework Core with MS SQL Server
* **Security:** JWT Authentication, BCrypt Password Hashing
* **Architecture Patterns:** Repository Pattern, Unit of Work, Dependency Injection

### Frontend
* **Framework:** Angular (using modern Stan
