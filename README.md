# Test Task
![image](https://github.com/olehkavetskyi/cool-kitties/assets/110283090/e224d3d5-3aa7-4785-8aaf-97a460125e2e)

## Description

This project is a web application that combines ASP.NET MVC for the backend and Angular for the frontend. It aims to test my skills and create URL shortener

## Technologies Used

ASP.NET MVC: A web application framework for building scalable and maintainable web applications using the Model-View-Controller architectural pattern.
Angular: A popular TypeScript framework for building single-page applications (SPAs) with a component-based architecture.
Entity Framework: An object-relational mapping (ORM) framework that simplifies database operations in ASP.NET applications.
Postgres SQL: A relational database management system used to store and retrieve data for the application.

## Tests
This project uses xUnit framework for backend and Jasmine for Angular.

## Setup Instructions
1. Clone the repository: git clone (https://github.com/olehkavetskyi/TestTask)
2. Set up the backend:
3. Open the solution file in Visual Studio.
4. Build the solution to restore NuGet packages and compile the code.
5. Configure the database connection string in the web.config file.
6. Run the database migrations to create the required tables.
7. Add this into appsettings.json:
   ```json
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=testtaskdb;User Id=postgres;Password=12#ubtr;"
      },
      "Token": {
        "Key": "super secret key",
        "Issuer": "https://localhost:7228/",
        "Audience": "https://localhost:7228/"
      }
```
