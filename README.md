# BudgetAPI

This is a **.NET 8** backend API for tracking expenses and budgets using **SQLite**. It allows users to manage their expenses, set budgets, and track their spending effectively.

## Features
- **User Management** (Register, Login, Update Profile)
- **Expense Management** (Add, Edit, Delete, Filter, Monthly Reports)
- **Budget Management** (Set, Update, Delete, Track Spending)
- **Spending Warnings** when approaching budget limits
- **Dependency Injection, DTOs, and AutoMapper for clean architecture**

## Technologies Used
- **.NET 8** (C#)
- **SQLite** (Database)
- **Entity Framework Core** (ORM)
- **Swagger UI** (API Testing)
- **AutoMapper** (DTO Mapping)

## Setup Instructions
### Prerequisites
- Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Install Visual Studio Community (or any IDE that supports .NET development)

### Clone the Repository
```sh
git clone https://github.com/your-username/ExpenseTrackerApi.git
cd ExpenseTrackerApi
```

### Configure Database
1. Open `appsettings.json` and ensure the SQLite connection string is correct:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=expenseTracker.db"
   }
   ```
2. Apply Migrations:
   ```sh
   dotnet ef database update
   ```

### Run the API
```sh
dotnet run
```
The API will start at `https://localhost:7093/` and open Swagger UI automatically.

## API Endpoints

### User Management
| Method | Endpoint       | Description            |
|--------|--------------|------------------------|
| POST   | `/api/user/register` | Register a new user  |
| POST   | `/api/user/login`    | User login          |
| PUT    | `/api/user/update`   | Update user profile |

### Expense Management
| Method | Endpoint          | Description                |
|--------|------------------|----------------------------|
| POST   | `/api/expenses`  | Add a new expense         |
| PUT    | `/api/expenses/{id}` | Update an expense    |
| DELETE | `/api/expenses/{id}` | Delete an expense    |
| GET    | `/api/expenses`  | Get all expenses          |
| GET    | `/api/expenses/{id}` | Get expense by ID   |

### Budget Management
| Method | Endpoint         | Description               |
|--------|-----------------|---------------------------|
| POST   | `/api/budgets`  | Add a new budget         |
| PUT    | `/api/budgets/{id}` | Update a budget     |
| DELETE | `/api/budgets/{id}` | Delete a budget     |
| GET    | `/api/budgets`  | Get all budgets          |
| GET    | `/api/budgets/{id}` | Get budget by ID   |

## Future Improvements
- Implement Authentication & Authorization
- Add GraphQL support
- Enhance reporting with charts & analytics

## License
This project is **open-source** under the [MIT License](LICENSE).

---

Feel free to modify and extend this API! 🚀
