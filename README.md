# 🍲 Yummly – Social Recipe Sharing Platform

Yummly is a community-driven app where users can share, like, and comment on recipes. It focuses on social interaction and was developed in parallel with my graduation project (Flighter).

## 🛠 Tech Stack

- .NET 8, ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- AutoMapper
- SQL Server
- IMemoryCache, MailKit

## 🔑 Core Features

### 🔐 Authentication System
- JWT-based login/registration
- Password reset with email verification using MailKit

### 📝 Recipe Posting
- Create, retrieve, and delete recipes
- Add images and categorize posts

### ❤️ Interactive Engagement
- Like/Unlike recipes
- Commenting system (edit/delete)
- Follow/unfollow users

### 🧠 Clean Architecture
- DTO-to-model mapping via AutoMapper
- Cloud-safe image upload with validation
- Role-based access control
