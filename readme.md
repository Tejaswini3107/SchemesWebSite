# SchemesWebSite

SchemesWebSite is an ASP.NET Core web application developed for managing schemes. It provides functionality for user authentication, scheme management, and multilingual support.

## Setup

To run this project locally, follow these steps:

1. **Clone the Repository**: 
    ```bash
    git clone https://github.com/Tejaswini3107/SchemesWebSite.git
    ```

2. **Configure Connection Strings**:
    - Open the `appsettings.json` file.
    - Update the `ConnectionStrings` section with your SQL Server connection string.

3. **Start SQL Server Developer Edition**:
    - Locate SQL Server Developer Edition in your list of installed programs or applications.
    - Double-click or select SQL Server Developer Edition to start it.

4. **Connect with SQL Server Management Studio (SSMS)**:
    - Open SQL Server Management Studio (SSMS). You can find it in your list of installed programs or applications.
    - Once SSMS is open, you'll be prompted to connect to a server.
    - Enter the server name. If SQL Server Developer Edition is installed locally, you can use one of the following options:
      - `(local)` or `.`: Refers to the local machine.
      - `localhost`: Refers to the local machine.
      - `YourComputerName`: Replace `YourComputerName` with the name of your computer.
    - Choose the authentication method:
      - **Windows Authentication**: Select this option if you want to authenticate using your Windows credentials.
      - **SQL Server Authentication**: Select this option if you want to authenticate using a SQL Server username and password.
    - Click "Connect" to establish the connection to the SQL Server instance.

5. **Run the Application**:
    - Open the solution in Visual Studio.
    - Build the solution and run the application using IIS Express or your preferred hosting method.

## Configuration

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "SqlCon": "Data Source=YOUR_SQL_SERVER_NAME_HERE;Initial Catalog=SchemesDatabase;Integrated Security=True;TrustServerCertificate=True"
  }
}
