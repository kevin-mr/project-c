# PROJECT C - API Integration Testing Tool

Welcome to the repository for **PROJECT C**. This guide will help you set up the project and run it locally on your machine.

## Features

- Create Mock Server endpoints to emulate a RESTful API behavior for testing during development.
- Capture Webhook events to redirect them to local environments using our CLI tool.
- Define variants of the simulated endpoints in the Mock Server to test multiple scenarios simultaneously.
- Generate responses dynamically from temporal storage and previous request data 

## Prerequisites

Before you begin, make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/) (or any IDE of your choice with .NET support)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or use [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for development purposes

## Getting Started

To get started with the project, follow these steps:

1. **Clone the repository**:

   ```bash
   git clone https://github.com/kevin-mr/project-c.git
   cd project-c

2. **Configure the Database**:

- Install and configure SQL Server or SQL Server Express if not already installed.
- Create a new database named `ProjectC-DB`.

3. **Database Connection Configuration**:

- Open the appsettings.json file in the ProjectC.Server project:
  ```
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=your_database_name;Trusted_Connection=True;MultipleActiveResultSets=true"
    },
    // Other configurations...
  }
  ```
- Replace `Server`, `Database`, and any other necessary settings with your SQL Server configuration.

4. **Run Database Migrations**:

- Open a terminal or command prompt in the ProjectC.Server directory and run the following commands:

  ```bash
  dotnet ef database update

- This command applies any pending migrations to the database.

5. **Build and Run the Project**:

- Open the solution (ProjectName.sln) in Visual Studio or Visual Studio Code.
- Restore the dependencies and build the solution.
- Set ProjectC.Server as the startup project.
- Press F5 or use the IDE's running/debugging feature to start the project.

6. **Access the Application**:

Once the project is running, you can access the Blazor application in your web browser at:

- https://localhost:7026 (or another port if configured differently)

## Troubleshooting

If you encounter any issues while setting up or running the project, please check the following:

- Ensure all prerequisites are installed correctly.
- Verify database connection strings and configurations in appsettings.json.
- Check for any build or runtime errors in Visual Studio or Visual Studio Code.

If your issue is not resolved, please submit an issue to the repository.
