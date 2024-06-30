# CandidateAPI

CandidateAPI is an ASP.NET Core web API project that manages candidate information. This document provides instructions for setting up the database using Entity Framework Core.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or a local instance like [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express))
- [Visual Studio](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

## Getting Started

1. **Clone the repository:**

    ```bash
    git clone https://github.com/sraman605/CandidateAPI.git
    cd CandidateAPI
    ```

2. **Update the connection string:**

    Open `appsettings.json` and update the `DefaultConnection` with your SQL Server connection details:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER;Database=CandidateDB;Trusted_Connection=True;"
      }
    }
    ```

## Setting Up the Database

1. **Install Entity Framework Core tools:**

    If you haven't already, install the EF Core CLI tools:

    ```bash
    dotnet tool install --global dotnet-ef
    ```

2. **Add EF Core packages:**

    Ensure your project includes the necessary EF Core packages in the `.csproj` file:

    ```xml
    <ItemGroup>
      <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.0" />
      <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />
    </ItemGroup>
    ```

3. **Create the initial migration:**

    Generate the initial migration to set up your database schema:

    ```bash
    dotnet ef migrations add InitialCreate
    ```

4. **Apply the migration to the database:**

    Apply the generated migration to your database to create the schema:

    ```bash
    dotnet ef database update
    ```

## Running the Application

1. **Build the project:**

    Build the project to ensure everything is set up correctly:

    ```bash
    dotnet build
    ```

2. **Run the application:**

    Run the project:

    ```bash
    dotnet run
    ```

    The API should now be running on `https://localhost:5001` (or a different port if configured).

## Additional Commands

- **Creating new migrations:**

    Whenever you make changes to your model classes, create a new migration:

    ```bash
    dotnet ef migrations add MigrationName
    ```

- **Updating the database:**

    Apply pending migrations to the database:

    ```bash
    dotnet ef database update
    ```

- **Removing the last migration:**

    If you need to remove the last migration (e.g., if it was created by mistake):

    ```bash
    dotnet ef migrations remove
    ```

## Troubleshooting

- **Connection Issues:**

    Ensure your connection string in `appsettings.json` is correct and your SQL Server instance is running.

- **Migrations Issues:**

    If you encounter issues with migrations, try deleting the `Migrations` folder and recreating the initial migration.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue to discuss changes.


