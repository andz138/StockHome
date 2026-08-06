# StockHome 

StockHome is a full-stack financial web application for searching public company data, managing personalized portfolios, and participating in user discussions. 

This project was built as a hands-on learning exercise to practice clean API design, relational database modeling, and unidirectional data flow in modern web applications.

---

## Architecture & Implementation Details

### Backend (ASP.NET Core Web API)
- **Custom Repository Layer:** Data access is abstracted behind interfaces (`IStockRepository`, `ICommentRepository`, `IPortfolioRepository`). While EF Core already acts as an ORM, this layer was explicitly implemented to practice decoupled architecture and separate SQL/eager-loading queries from controller logic.
- **Manual Extension Mappers:** Object mapping is handled using explicit C# static extension methods (e.g., `ToStockDto()`). This was chosen over libraries like AutoMapper to maintain compile-time safety, easy debugging, and eliminate reflection overhead.
- **Relational Database Design (EF Core & SQL Server):**
  - **One-to-Many:** Stocks and Comments are linked with eager loading (`Include()`) and cascade delete configurations.
  - **Many-to-Many:** Users and Stocks are associated through a composite-key join table (`Portfolio`) using Fluent API configurations.
  - **One-to-One:** User identity linked directly to user-generated comments.
- **Server-Side Queries:** Implemented query helper objects to process custom database-level **Filtering**, **Sorting**, and **Pagination** parameters directly on EF Core queryables.
- **Validation & Route Constraints:** Strict request validation using DTO Data Annotations and explicit `ModelState` checks inside API endpoints.
- **Security:** Integrated ASP.NET Core Identity with role seeding and custom JWT token generation.

### Frontend (React + Vite + TypeScript)
- **State Flow (Lifting State Up):** Designed around logical parent components that manage state and fetch API data, passing read-only props down to stateless presentational child components (`Search`, `Card`, `CardList`).
- **External Data Fetching:** Axios-driven service layer utilizing TypeScript generic interfaces to communicate with the **Financial Modeling Prep (FMP) API** securely.
- **Type Safety:** Defined custom TypeScript interfaces (`company.d.ts`) to strictly type third-party API payloads.

---

## Tech Stack

| Domain | Technologies |
| :--- | :--- |
| **Frontend** | React, Vite, TypeScript, Axios, Tailwind CSS |
| **Backend** | .NET Core (Web API), Entity Framework Core, Newtonsoft.Json |
| **Database** | Microsoft SQL Server |
| **Security** | ASP.NET Core Identity, JWT (JSON Web Tokens) |
| **Integrations** | Financial Modeling Prep (FMP) API, Swagger (OpenAPI) |

---

## Codebase Directory Layout

### Backend (`api/`)
```
api/
├── Controllers/       # API HTTP endpoints (Stock, Comment, Portfolio, Account)
├── Data/              # ApplicationDbContext, DB configurations, and Migrations
├── Dtos/              # Request/Response Data Transfer Objects (DTOs)
├── Extensions/        # Claims and identity helper extensions
├── Interfaces/        # Repository contracts (IStockRepository, etc.)
├── Mappers/           # Manual static extension mappers
├── Models/            # Database domain models (Stock, Comment, AppUser, Portfolio)
├── Repository/        # Concrete EF Core repository implementations
├── Services/          # Token generation service
└── Program.cs         # App bootstrap, middleware pipeline, and dependency injection
```

### Frontend (`frontend/`)
```
frontend/
├── src/
│   ├── Components/    # Presentational/Dumb Components (Card, CardList, Search)
│   ├── Pages/         # Logical/Smart Components managing state and API flow
│   ├── Services/      # Axios service layer for FMP API endpoints
│   ├── Types/         # TypeScript declarations (company.d.ts)
│   ├── App.tsx        # Central entry-point holding root state and layout
│   └── index.tsx      # React DOM bootstrap
└── .env               # Local environment configurations (Vite API Key)
```

---

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server/)
- A free API Key from [Financial Modeling Prep](https://site.financialmodelingprep.com/)

---

### Backend Setup

1. **Navigate to the API folder:**
   ```bash
   cd api
   ```

2. **Configure Database Connection & JWT Keys:**
   Configure your connection string and JWT settings in `appsettings.json`:
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
   Interactive Swagger documentation will be available at: `https://localhost:PORT/swagger`.

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
   Create a `.env` file directly inside the `frontend` folder:
   ```env
   VITE_API_KEY=your_financial_modeling_prep_api_key
   ```

4. **Run the Development Server:**
   ```bash
   npm run dev
   ```
   The application will spin up at: `http://localhost:5173`.
