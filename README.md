# TaskManagementAPI

# Project Overview
This is a simple Task Management API built with .NET 8 and Entity Framework Core, using a SQL Server database.
It includes basic JWT token authentication and role-based authorization (User, Manager).

# Features
Create Task (POST /tasks)

Get Task by ID (GET /tasks/{id})

Get Tasks by User ID (GET /tasks/user/{userId})

JWT Authentication (login and secure endpoints)

Role-based access control (User, Manager)

Entity Framework Core (SQL Server)

Swagger UI for easy API testing

Model Validation for input data

Seed Data for initial users and tasks

# Technologies Used
.NET 8

Entity Framework Core

SQL Server

JWT (JSON Web Token) Authentication

Swagger (Swashbuckle)

# Repository
GitHub Repository: TaskManagementAPI

# Getting Started
# Prerequisites
.NET 8 SDK

SQL Server or SQL Server Express installed locally

Visual Studio 2022 / Visual Studio Code

# Installation and Setup
Clone the repository:
git clone https://github.com/Bapuji999/TaskManagementAPI.git
cd TaskManagementAPI

Restore NuGet packages:
dotnet restore
Apply Migrations to Database: Open Package Manager Console in Visual Studio and run:
Update-Database
This will create the database and apply all migrations.

Run the Project:
dotnet run
Open Swagger UI to test:
https://localhost:{port}/swagger
Authentication
You need to login first to get a JWT Token and use it to authorize further API calls.

Seeded Users:
| Username | Password | Role |
|-----------------|-----------------|-----------------|
| john.doe | Mabc@123     | Manager    |
| jane.smith | Abc@123     | User    |

Use the /login endpoint to get a token.

Set the token in Swagger using Authorize button (Bearer {token}).
