# MovieApp

## Version: 1.0.0 Stable

### About MovieApp:

**MovieApp** is a dynamic web application designed for efficient movie management. It seamlessly integrates with Entity Framework for robust database interactions, providing users with an optimized and reliable experience. With comprehensive documentation and advanced error handling, MovieApp ensures a user-friendly experience.

Users can effortlessly manage their movie collections, including adding, filtering, updating, and deleting movies. The application features extensive user account management capabilities, including registration, authentication, password management, and profile updates. The clean codebase and use of dependency injection ensure maintainability and extensibility. Custom exception handling improves error reporting, making MovieApp a dependable choice for movie organization and user management needs.

### Key Features:

- Entity Framework Integration for Reliable Database Management
- Clean Codebase and Dependency Injection for Maintainability and Extensibility
- Custom Exceptions for Enhanced Error Reporting and Handling
- Built on the principles of Object-Oriented Programming (OOP)
- Comprehensive XML Documentation for Clear API Understanding
- Code Optimization for Improved Performance
- User Account Management Functionality

### How to Run the App:

Follow these steps to run MovieApp locally:

1. **Clone the Repository:** Start by cloning this repository to your local machine using the following command: `git clone https://github.com/Biohazardx44/Web-API-Projects.git`
2. **Install Dependencies:** Ensure you have the necessary dependencies installed, including [[Visual Studio]](https://visualstudio.microsoft.com/downloads/), [[SQL Server/ SSMS]](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) and [[Postman]](https://www.postman.com/downloads/)
3. **Start the Solution:** Navigate to the directory where you cloned the repository and open the solution in Visual Studio.
4. **Change the ConnectionString:** Navigate to `MovieApp.Api > appsettings.json` and update the connection string to match your local database setup.
5. **Open NuGet Package Console:** Navigate to `Tools > NuGet Package Manager > Package Manager Console`
6. **Setup Database:** Set `MovieApp.DataAccess` as the default project in the console, then initialize the database using the `add-migration <DB Name>` and `update-database` commands.
7. **Test Endpoints with Swagger and Postman:** To test with Swagger, click on the `MovieApp.Api` button or find the URL in `MovieApp.Api > Properties > launchSettings.json`. For Postman, launch the application and use the URL from the same location.