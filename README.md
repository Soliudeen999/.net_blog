# Blog API

A modern, feature-rich Blog API built with ASP.NET Core 8, Entity Framework Core, and MySQL. This API provides a complete blogging platform with user authentication, post management, comments, reactions, and tagging system.

## 🚀 Features

### Core Features
- **User Authentication** - Session-based authentication with password hashing
- **Blog Posts Management** - Full CRUD operations with automatic slug generation
- **Comments System** - Hierarchical comments with replies support
- **Reactions System** - Multiple reaction types (Like, Love, Laugh, etc.)
- **Tagging System** - Flexible tagging for posts
- **Search & Filtering** - Advanced search and filtering capabilities
- **Pagination** - Built-in pagination for all list endpoints

### Technical Features
- **RESTful API** - Clean, RESTful design with proper HTTP status codes
- **Swagger Documentation** - Interactive API documentation
- **Input Validation** - Comprehensive request validation
- **Error Handling** - Global exception handling with proper error responses
- **Database Seeding** - Sample data for testing
- **CORS Support** - Cross-origin resource sharing enabled

## 🛠️ Tech Stack

- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web framework
- **Entity Framework Core** - ORM for database operations
- **MySQL** - Database
- **Swagger/OpenAPI** - API documentation
- **Session Authentication** - User session management

## 📋 Prerequisites

Before running this project, make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (8.0 or higher)
- [Git](https://git-scm.com/) (for cloning the repository)

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone <repository-url>
cd Blog
```

### 2. Database Setup

1. **Create MySQL Database:**
   ```sql
   CREATE DATABASE c_blog;
   ```

2. **Update Connection String:**
   Edit `appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "AppDbConnectionString": "server=localhost;Database=c_blog;User=root;Password=your_password"
     }
   }
   ```

3. **Run Database Migrations:**
   ```bash
   dotnet ef database update
   ```

### 3. Run the Application

```bash
dotnet run
```

The API will be available at:
- **API Base URL:** `http://localhost:5033/api/v1`
- **Scalar Documentation:** `http://localhost:5033/scalar`

## 📚 API Documentation

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/register` | Register a new user |
| POST | `/api/v1/login` | Login user |
| GET | `/api/v1/check-session` | Check current session |
| POST | `/api/v1/logout` | Logout user |

### Posts Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/posts` | List all posts (with filtering) |
| GET | `/api/v1/posts/{slug}` | Get post by slug |
| POST | `/api/v1/posts` | Create new post |
| PUT | `/api/v1/posts/{id}` | Update post |
| DELETE | `/api/v1/posts/{id}` | Delete post |

### Comments Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/posts/{postId}/comments` | Get comments for a post |
| POST | `/api/v1/posts/{postId}/comments` | Add comment to a post |
| PUT | `/api/v1/comments/{id}` | Update comment |
| DELETE | `/api/v1/comments/{id}` | Delete comment |

### Reactions Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/posts/{postId}/reactions` | Get reactions for a post |
| POST | `/api/v1/posts/{postId}/reactions` | Add reaction to a post |
| DELETE | `/api/v1/posts/{postId}/reactions/{reactionId}` | Remove reaction |
| GET | `/api/v1/posts/{postId}/reactions/summary` | Get reaction summary |

### Tags Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/tags` | List all tags |
| POST | `/api/v1/tags` | Create new tag |
| DELETE | `/api/v1/tags/{id}` | Delete tag |

## 🔐 Authentication

The API uses session-based authentication. After login, a session cookie is set that must be included in subsequent requests.

### Sample Authentication Flow

1. **Login:**
   ```bash
   curl -X POST http://localhost:5033/api/v1/login \
     -H "Content-Type: application/json" \
     -d '{
       "email": "john@example.com",
       "password": "password123"
     }' \
     -c cookies.txt
   ```

## 📝 Sample Data

The application comes with pre-seeded sample data:

### Sample Users
- **John Doe** (`john@example.com`) - Password: `password123`
- **Jane Smith** (`jane@example.com`) - Password: `password123`

### Sample Posts
- "Getting Started with ASP.NET Core"
- "Entity Framework Core Best Practices"

### Sample Tags
- Technology, Programming, Web Development, C#, ASP.NET Core

## 🔧 Configuration

### Environment Variables

You can override configuration using environment variables:

```bash
export ASPNETCORE_ENVIRONMENT=Development
export ConnectionStrings__AppDbConnectionString="server=localhost;Database=c_blog;User=root;Password=your_password"
```

### App Settings

Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "AppDbConnectionString": "server=localhost;Database=c_blog;User=root;Password=password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🧪 Testing

### Using Scalar Documentation

1. Navigate to `http://localhost:5033/scalar`
2. Explore the comprehensive API documentation
3. Test endpoints directly from the interface

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [troubleshooting](#troubleshooting) section
2. Open an issue in the repository

---

**Happy Blogging! 🚀** 