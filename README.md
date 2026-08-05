# StockHome 

StockHome is a high-performance, full-stack financial web application that allows users to search for public companies, view detailed stock data, manage a personalized portfolio, and engage with other users through a structured comment system. 

The project features a modern **React (Vite + TypeScript)** frontend coupled with a robust **ASP.NET Core Web API** backend, utilizing **Entity Framework Core**, **SQL Server**, and secure **JWT-based identity management**.

---

## Key Features

### Backend (ASP.NET Core Web API)
- **Repository Pattern & Dependency Injection:** Decouples data access from business logic, ensuring testability, maintainability, and clean architecture.
- **ASP.NET Core Identity & JWT Authentication:** Secure registration, login, and authorization. Supports role-based access control with seeded user roles.
- **Relational Database Design:** 
  - **One-to-Many:** Stocks and Comments with eager loading (`Include`) and cascade delete configuration.
  - **Many-to-Many:** Users and Stocks connected via a composite-key `Portfolio` join table.
  - **One-to-One:** Linked user-generated content for comments.
- **Advanced Query Support:** Built-in helper objects enabling server-side **Filtering**, **Sorting**, and **Pagination**.
- **Java-Esque Extension Mappers:** Custom static extension methods for manual object-to-DTO mapping, avoiding the runtime risks, "magic" configurations, and reflection overhead of libraries like AutoMapper.
- **Robust Validation:** Strict route constraints and DTO data annotations validated via controller `ModelState` checks.
- **Swagger Documentation:** Configured to support JWT authentication headers, making API testing seamless.

### Frontend (React + Vite + TypeScript)
- **Modern Vite Build Tooling:** Fast HMR (Hot Module Replacement) and optimized build times compared to legacy templates.
- **"Tree and River" Architecture:** Structured around smart (stateful/logical) parent components and dumb (stateless/presentational) child components for efficient, predictable data rendering.
- **Financial Modeling Prep (FMP) API Integration:** Axios-driven service layer utilizing TypeScript generic interfaces and safe environment variables to securely fetch real-time financial markets data.
- **Clean Component Structure:** Dedicated directories for components (`Card`, `CardList`, `Search`) and pages.

---

## Tech Stack

| Domain | Technologies |
| :--- | :--- |
| **Frontend** | React, Vite, TypeScript, Axios, Tailwind CSS / Custom CSS |
| **Backend** | .NET (ASP.NET Core Web API), Entity Framework Core (EF Core), Newtonsoft.Json |
| **Database** | Microsoft SQL Server |
| **Security** | ASP.NET Core Identity, JWT (JSON Web Tokens) |
| **Integrations** | Financial Modeling Prep (FMP) API, Swagger (OpenAPI) |

---

## Getting Started

### Prerequisites
Before running the application locally, make sure you have installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [SQL Server (LocalDB or Express)](https://www.microsoft.com/sql-server/)
- A free API Key from [Financial Modeling Prep](https://site.financialmodelingprep.com/)

---

### Backend Setup

1. **Navigate to the API folder:**
   ```bash
   cd api
   ```

2. **Configure Database Connection & JWT Keys:**
   Update the connection string and JWT properties in your `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=StockHome;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "JWT": {
       "Issuer": "your-issuer",
       "Audience": "your-audience",
       "SigningKey": "your-very-long-secure-and-random-signing-key-here"
     }
   }
   ```

3. **Install Dependencies & Apply Migrations:**
   ```bash
   dotnet restore
   dotnet ef database update
   ```

4. **Run the API:**
   ```bash
   dotnet watch run
   ```
   The backend will start and watch for file changes. You can access the interactive Swagger documentation at: `https://localhost:PORT/swagger`.

---

### Frontend Setup

1. **Navigate to the frontend folder:**
   ```bash
   cd frontend
   ```

2. **Install Dependencies:**
   ```bash
   npm install
   ```

3. **Configure Environment Variables:**
   Create a `.env` file directly inside the `frontend` folder and add your FMP API Key:
   ```env
   VITE_API_KEY=your_financial_modeling_prep_api_key
   ```

4. **Run the Development Server:**
   ```bash
   npm run dev
   ```
   This will spin up the development server (typically at `http://localhost:5173`).

---

## Project Structure

### Backend Architecture
```
api/
├── Controllers/       # API endpoints (Stock, Comment, Portfolio, Account)
├── Data/              # ApplicationDbContext, DB configurations, and Migrations
├── Dtos/              # Data Transfer Objects (Requests, Responses, Auth)
├── Extensions/        # Claim Extensions, custom utilities
├── Interfaces/        # Repository interfaces (IStockRepository, etc.)
├── Mappers/           # Manual static extension mappers
├── Models/            # Domain models (Stock, Comment, AppUser, Portfolio)
├── Repository/        # Concrete implementations of repository interfaces
├── Services/          # Token generation and business services
└── Program.cs         # Dependency injection, middleware pipeline, and JWT configuration
```

### Frontend Architecture
```
frontend/
├── src/
│   ├── Components/    # Dumb/Presentational Components (Card, CardList, Search)
│   ├── Pages/         # Smart/Logical Components managing state and API flow
│   ├── Services/      # Axios API configuration & FMP endpoints
│   ├── Types/         # TypeScript declaration files (company.d.ts)
│   ├── App.tsx        # Central entry-point holding root layout and state
│   └── index.tsx      # Application bootstrapper
└── .env               # Environment configuration files
```

---

## Security & Authorization

All core endpoints of StockHome are protected by role-based authorization. When querying stocks or modifying portfolios, the user must register, log in, and acquire a JWT bearer token. This JWT is then parsed securely on the client-side to associate user identities with their corresponding comments and portfolios.

## Contributing

Contributions, issues, and feature requests are welcome! Feel free to check the issues page if you want to contribute to the StockHome codebase.


