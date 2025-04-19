
# 🚗 Smart Toll System API

A backend system for a **Smart Toll Payment** solution that enables automatic toll deduction via vehicle detection. Built using **.NET 9**, **Clean Architecture**, and **JWT Authentication**, this API supports both **Vehicle Owners** and **Admins**, along with a **Radar System** integration.

---

## 🏗️ Tech Stack

- **.NET 9**
- **Clean Architecture** (Domain, Application, Infrastructure, API)
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Identity**
- **JWT Authentication** with RS256
- **Swagger / Swashbuckle**
- **CORS Configuration**
- **RESTful API**

---

## 📁 Project Structure

```
SmartTollSystem/
│
├── SmartTollSystem.Domain         # Entities and core interfaces
├── SmartTollSystem.Application    # Business logic and use cases
├── SmartTollSystem.Infrastructure # Data access, services, Identity
├── SmartTollSystem.API            # API layer (controllers, middleware)
└── SmartTollSystem.Tests          # Unit and integration tests
```

---

## 🎯 Features

### 🔐 Authentication & Roles
- JWT-based login and access
- Role-based authorization (`Admin`, `VehicleOwner`, `RadarSystem`)
  
### 🚗 Vehicle Owner
- Register and authenticate
- Add and manage vehicles
- View toll history and payments

### 👮 Admin
- View all users, vehicles, and transactions
- Manage toll rates
- Control system settings

### 📡 Radar System
- Detect vehicle via license plate
- Trigger automatic toll deduction
- Send payment request to backend

---

## 🚀 Getting Started

### ✅ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code

### 🧪 Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/SmartTollSystem.git
   cd SmartTollSystem
   ```

2. **Update the Connection String**
   In `appsettings.json` (under `SmartTollSystem.API`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=SmartTollDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Apply Migrations**
   ```bash
   cd SmartTollSystem.API
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   dotnet run --project SmartTollSystem.API
   ```

5. **Test in Swagger**
   Navigate to `https://localhost:7070/swagger` and explore the API.

---

## 🔑 JWT Authentication

- RS256 (asymmetric encryption)
- Includes:
  - Login and register endpoints
  - Secure API access for authenticated users
  - JWT tokens stored client-side

---

## 🧪 API Demo Use Case

- **Python or Arduino client** sends plate to:
  ```http
  POST /api/radar/detect
  Authorization: Bearer <JWT>
  {
    "plateNumber": "ABC1234"
  }
  ```

- Server:
  - Verifies plate
  - Deducts toll from balance
  - Logs transaction

---

## ⚙️ Future Enhancements

- Email/SMS payment confirmations
- Admin web dashboard
- Payment gateway integration
- ML model for vehicle type detection

---

## 🙌 Authors

- Yahia: [Yahya](https://github.com/Yahiasherif002)
- Yousef Ali Mansour: [Yousef Ali Mansour](https://github.com/yousefalimansour)
