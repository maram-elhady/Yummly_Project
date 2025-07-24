# 🍽️ Yummly – Recipe Sharing & Social App (Backend)

## 📌 Overview
Yummly is a **social recipe-sharing platform** that allows users to:
- Create and manage recipe posts with images and categories.
- Follow other users, like posts, and add comments.
- Search recipes by category or user.

It also features **secure authentication**, **JWT-based authorization**, **email verification for password reset**, and **pagination support**.

---

## ✅ Features

### 🔐 Authentication
- ✅ User registration & login with **JWT tokens**
- ✅ Role-based authorization (**User**, **Admin**)
- ✅ Forgot Password flow with **email verification** using **MailKit**
- ✅ Secure password reset with **temporary tokens**

### 👥 Social Features
- ✅ Follow/Unfollow users
- ✅ Like/Unlike posts (toggle)
- ✅ Add, edit, delete comments on posts
- ✅ Retrieve followers & following list

### 📝 Posts & Recipes
- ✅ Create, update, delete posts with:
  - Image Upload (**validations for size and format**)
  - Ingredients & Description
- ✅ Fetch user posts
- ✅ Pagination support for listing posts

### 🔍 Search
- ✅ Search Users:
  - Prioritized results: **Followed users → Followers → Others**
- ✅ Search Recipes by Category:
  - Pagination support
  - Includes **metadata (total count, current page, page size)**

### 📧 Email Integration
- ✅ Send **verification codes** for password reset
- ✅ HTML template for emails (**styled and responsive**)

---

## 🛠️ Tech Stack
- **.NET 8 (Web API)**
- **Entity Framework Core (Database ORM)**
- **SQL Server (Database)**
- **Identity (User & Role Management)**
- **MailKit (Email sending)**
- **AutoMapper (DTO mapping)**
- **MemoryCache (Temporary data storage for verification codes)**

---

## 📂 Project Structure
```
Yummly/
├── Controllers/         # API Controllers
├── DTO/                 # Data Transfer Objects
├── Services/            # Business Logic Layer
│   ├── Authentication   # AuthService for login/register/reset
│   ├── PostService      # Create/Delete/Get Posts
│   ├── FollowService    # Follow/Unfollow
│   ├── ActivityService  # Likes & Comments
│   ├── SearchService    # Search Users & Categories
├── Models/              # EF Core Models
├── Helper/              # JWT, Email Config, Response Wrappers
└── wwwroot/             # Uploaded Images
```

---

## ⚙️ Setup & Installation

### ✅ Prerequisites
- .NET 7 SDK
- SQL Server
- Visual Studio / VS Code

### ✅ Steps
1. **Clone the repository**
```bash
git clone https://github.com/your-username/yummly-api.git
cd yummly-api
```

2. **Set up Database**
- Update `appsettings.json` with your **SQL Server connection string**
- Run migrations:
```bash
dotnet ef database update
```


---

## 🔐 Authentication Flow
- **Register** → JWT Token issued
- **Login** → JWT Token issued
- **Forgot Password**:
  - Request → Verification Code sent via Email
  - Verify Code → Token generated in cache
  - Reset Password → Validate token & update password

---
## 🚀 Future Enhancements
- Real-time chat using **SignalR**
