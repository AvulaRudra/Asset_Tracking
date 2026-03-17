# PostgreSQL Setup for Asset Tracker (.NET Backend)

## Prerequisites
- PostgreSQL 15+ installed locally or running in Docker
- .NET 8 SDK

## 1. PostgreSQL Installation

### Option A: Local Install (Windows)
1. Download PostgreSQL from https://www.postgresql.org/download/windows/
2. Run installer and remember password (default: `postgres`)
3. Ensure pgAdmin 4 is installed (included)

### Option B: Docker
```bash
docker run --name postgres-assettracker -e POSTGRES_PASSWORD=Rudra@123 -p 5432:5432 -d postgres:15
```

## 2. Database Setup
1. Open pgAdmin or `psql` as postgres user
2. Create database:
```sql
CREATE DATABASE asset_tracker;
```

## 3. Configuration
1. Copy `.env.example` to `.env` and update password:
```bash
cp .env.example .env
# Edit .env: set ConnectionStrings__DefaultConnection password
```

2. The .NET app will read `.env` automatically (via DotNetEnv)

## 4. EF Core Migrations
```bash
# Install EF Core tools (once)
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

## 5. Run Application
```bash
dotnet run
```
The app will connect to PostgreSQL using credentials from `.env`.

## 6. Verify Tables
Connect to `asset_tracker` database and verify:
- `Users` table (SSO users)
- `LocalUsers` table (local auth)

## Troubleshooting
- If connection fails, check PostgreSQL service is running
- Ensure password in `.env` matches PostgreSQL user password
- For Docker, ensure container is running: `docker ps`
