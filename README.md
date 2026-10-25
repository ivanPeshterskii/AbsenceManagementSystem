# AbsenceManagementSystem

## A quick overview

This project will be used to help the teachers in the Draginovo's kindergarden ["ДГ - Пролет с. Драгиново"](https://prolet-dg.eu). Here teachers can add their froups and manage pupils' absences easily. The system is quite simple, they can do all the CRUD operations and manage their work. 

## Tech Stack

| Layer     |   Technology  |
| ----------| :----------- |
| Backend   | C# / ASP.NET Core MVC |
| ORM       | Entity Framework 6 / Core |
| Database  | SQL Server (Docker Host) |
| Frontend  | Razor views |
| IDE       | Visual Studio for Mac / Visual Studio Code |
| Version Control | Git & GitHub |

## Database Schema

The application uses **Entity Framework Core** with **SQL Server**

The database consists of four main entities:

- `Group`
- `Child`
- `Teacher`
- `Absence`

### Group
Represents a kindergarten group.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int | Yes       | PK        |
| Name      | string | Yes    | Name of group |
| AgeGroup  | int  | Yes      | Age group of the children |

### Child
Represents a child, attending the kindergarten.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| FirstName | string | Yes    | Child's first name|
| LastName  | string | Yes    | Child's last name |
| GroupId   | int     | Yes    | Fk to Group      |

### Teacher
Represents a teacher, assigned to the kindergarten group.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| FirstName | string | Yes    | Teacher's first name|
| LastName  | string | Yes    | Teacher's last name |
| GroupId   | int     | Yes    | Fk to Group      |

### Absence
Represents an absence, registered for a child.

| Property | Type | Required | Description |
| --------- | ---- | -------- | ---------- |
| Id        | int  |   Yes    | PK         |
| Date      | DateTime | Yes  | Date of the absence |
| Type      | AbsenceType | Yes | Type of absence |
| Note      | string?     | No  | Optional additional information |

### AbsenceType

```csharp
public enum AbsenceType 
{
    Unexcused = 1,
    Excused = 2,
    Medical = 3
}
```

### Business logic

All the business logic will be separated into `services` that implements a specific interface. 

A piece of the project worktree:

```text
AbsenceManagementSystem/
│
├── Services/
│   │
│   ├── Contracts/
│   │   ├── IChildService.cs
│   │   ├── IGroupService.cs
│   │   ├── ITeacherService.cs
│   │   └── IAbsenceService.cs
│   │
│   ├── ChildService.cs
│   ├── GroupService.cs
│   ├── TeacherService.cs
│   └── AbsenceService.cs
│
└── Program.cs
```

## Getting Started

These instructions use **SQL Server in Docker** so the database setup is consistent on macOS, Windows, and Linux. The .NET application itself runs on the host machine, not inside Docker.

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) (the project targets .NET 6; note that .NET 6 is out of support)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) on macOS/Windows, or Docker Engine on Linux
- Git (if cloning the repository)
- EF Core CLI tools (version compatible with EF Core 6)

> SQL Server's official Linux container image requires a compatible CPU architecture. On Apple Silicon Macs, check Docker image compatibility; x86-64 emulation may be required and is not guaranteed to work on every setup.

### 1. Obtain the source code

Clone the GitHub repository or download and extract its ZIP archive. Open a terminal in the folder containing the application's `.csproj` file.

### 2. Start SQL Server

Start Docker, then run this **single-line command** in a terminal (macOS/Linux shell or Windows PowerShell):

```bash
docker run --name absence-sql -e ACCEPT_EULA=Y -e MSSQL_SA_PASSWORD=ReplaceWith_StrongPassword123! -p 127.0.0.1:1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Choose your **own strong password** instead of the example. The SQL Server password must satisfy SQL Server's password policy. The `127.0.0.1` binding keeps the database port local to the host. Wait until SQL Server is ready before continuing.

To check the container:

```bash
docker ps
docker logs absence-sql
```

For later runs, restart the existing container with `docker start absence-sql` instead of creating another one. If port 1433 is occupied, change the *host* port (e.g. `1434:1433`) and use `localhost,1434` in the connection string.

### 3. Configure the database connection

Use the same SQL Server Authentication connection-string **format** on macOS, Windows, and Linux:

```text
Server=localhost,1433;Database=AbsenceSystemDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;
```

`YOUR_PASSWORD` must match the password chosen in step 2. This format is cross-platform, but the database server, port and credentials still need to match the reviewer's environment. `TrustServerCertificate=True` is intended for this local development setup, not production.

**Recommended: .NET user secrets (all platforms)**

From the directory containing the application's `.csproj` file:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=AbsenceSystemDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

Replace `YOUR_PASSWORD`. User secrets are loaded when the application runs in the `Development` environment. They are stored outside the project directory, so credentials do not need to be committed to GitHub.

**Alternative: environment variable**

macOS/Linux (bash/zsh):

```bash
export ConnectionStrings__DefaultConnection='Server=localhost,1433;Database=AbsenceSystemDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;'
```

Windows PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=localhost,1433;Database=AbsenceSystemDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;'
```

Set the variable in the same terminal session used to run `dotnet` commands. The double underscore (`__`) is supported across operating systems by ASP.NET Core configuration. Environment variables override `appsettings.json` and user secrets.

> The existing `appsettings.json` uses Windows integrated authentication (`Trusted_Connection=True`). The configuration above overrides it for a Docker-based SQL Server. Never commit real passwords to source control.

### 4. Restore dependencies and apply migrations

From the application project directory:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 6.0.0
dotnet ef database update
dotnet build
```

If `dotnet-ef` is already installed, skip its installation. If there are multiple `.csproj` files, use the application project as the working directory or specify `--project` and `--startup-project` explicitly.

### 5. Run the application

```bash
dotnet run
```

Open the localhost URL printed in the terminal. In the `Development` environment, the application is also configured to apply migrations and seed initial data at startup. A running SQL Server instance and a valid connection string are required.

### Troubleshooting

| Problem | What to check |
| --- | --- |
| SQL connection refused / server not found | Docker is running; `docker ps` shows `absence-sql`; port matches the connection string |
| Login failed for user `sa` | Password in connection string matches the password used to create the container |
| Certificate / SSL error | Local connection string includes `TrustServerCertificate=True` |
| `dotnet ef` not found | Install the compatible `dotnet-ef` CLI tool |
| Migration or database errors | Verify the SQL Server container is ready, then retry `dotnet ef database update` |
| Container name already exists | Use `docker start absence-sql` instead of repeating `docker run` |

## Project Structure

The application follows the ASP.NET Core MVC pattern:

- **Models:** Represent application data and database entities.
- **Views:** Razor pages that display information and provide forms.
- **Controllers:** Handle HTTP requests and coordinate application operations.
- **Data / Entity Framework Core:** Manage database access and migrations.
- **Services / DTOs:** Support application logic and data transfer where used.
- **wwwroot:** Contains static frontend assets.

## AI Usage Disclosure

I used **ChatGPT** as an assistance tool during development, primarily for:

- Help with the presentation and structure of Razor Views.
- Guidance when writing and adjusting Bootstrap classes and responsive layouts.
- Debugging support, including understanding error messages and identifying possible fixes.

AI suggestions were used as development assistance and were reviewed and adapted for the project. This disclosure describes the areas where AI was used; it is not a claim that the application was entirely generated by AI.

## Notes

This is an educational project intended to demonstrate the fundamentals of ASP.NET Core MVC. The current scope focuses on core data management rather than production deployment or user authentication.
