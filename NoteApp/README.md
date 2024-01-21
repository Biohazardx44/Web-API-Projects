# NoteApp

## Version: 1.2.3 Stable

### About NoteApp:

**NoteApp** is a versatile web application designed for efficient note management. It seamlessly integrates with multiple databases (Entity Framework, Dapper, and AdoNet) to provide users with optimal performance and flexibility. With comprehensive documentation and reliable error handling, NoteApp offers a user-friendly experience, now enhanced with Serilog for advanced logging.

Users can manage their notes effortlessly and securely, ensuring organized and efficient information storage. In addition, NoteApp features extensive user account management capabilities, including registration, authentication, password management, and profile updates. The clean codebase and dependency injection make the application maintainable and extensible. Custom exception handling enhances error reporting, making NoteApp a reliable choice for note organization and user management needs.

### Key Features:

- Entity Framework, Dapper, and AdoNet Integration for Flexible Database Management
- Clean Codebase and Dependency Injection for Maintainability and Extensibility
- Custom Exceptions for Improved Error Reporting and reliable Error Handling
- Built on the principles of Object-Oriented Programming (OOP)
- Comprehensive XML Documentation for Clear API Understanding
- Code Optimization for Enhanced Performance
- Serilog Integration for Advanced Logging
- User Account Management Functionality

### How to Run the App:

Follow these steps to run NoteApp locally:

1. **Clone the Repository:** Start by cloning this repository to your local machine using the following command: `git clone https://github.com/Biohazardx44/Web-API-Projects.git`
2. **Install Dependencies:** Ensure you have the necessary dependencies installed, including [[Visual Studio]](https://visualstudio.microsoft.com/downloads/), [[SQL Server/ SSMS]](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) and [[Postman]](https://www.postman.com/downloads/)
3. **Start the Solution:** Navigate to the directory where you cloned the repository and open the solution in Visual Studio
4. **Change the ConnectionString:** Navigate to `NoteApp.Api > appsettings.json` and update the connection string to match your local database setup
5. **Open NuGet Package Console:** Navigate to `Tools > NuGet Package Manager > Package Manager Console`
6. **Setup Database:** Set `NoteApp.DataAccess` as the default project in the console, then initialize the database using the `add-migration <DB Name>` and `update-database` commands
7. **Test Endpoints with Swagger and Postman:** To test with Swagger, click on the `NoteApp.Api` button or find the URL in `NoteApp.Api > Properties > launchSettings.json`. For Postman, launch the application and use the URL from the same location