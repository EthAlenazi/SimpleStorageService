SimpleStorageService

SimpleStorageService is a lightweight and straightforward storage service built using modern .NET technologies. This project serves as a practical implementation for managing file storage efficiently, demonstrating essential concepts in backend development.

Features

Upload and download files.

Multiple storage options:

Amazon S3 Compatible Storage Service

Database Table

Local File System

FTP (Bonus Points)

Simple API endpoints for managing file storage.

Built with .NET, ensuring robust and scalable architecture.

Adheres to clean code principles, ensuring readability and maintainability.

Implements Factory and Strategy design patterns for flexible and extensible storage handling.

Integrated JWT (JSON Web Token) authentication for secure API access.

Prerequisites

.NET 8.0 SDK or later.

Visual Studio or any other preferred code editor.

A basic understanding of ASP.NET Core Web API.

Setup

Clone the repository:

git clone https://github.com/EthAlenazi/SimpleStorageService.git

Navigate to the project directory:

cd SimpleStorageService

Restore dependencies:

dotnet restore

Run the application:

dotnet run

Access the API locally at https://localhost:5001.

Usage

Use tools like Postman or Swagger UI (provided with the project) to test the API.

Example endpoints:

POST /v1/blobs: Upload a file.

Send the file in the request body as multipart/form-data.

GET /v1/blobs/<id>: Retrieve a file by its ID.

POST /authenticate: Obtain a JWT token for secure access.

Include the JWT token in the Authorization header for protected endpoints.

Contributing

Contributions are welcome! Feel free to fork the repository, make improvements, and submit a pull request.

Acknowledgments

Special thanks to the open-source community for providing the tools and inspiration to develop this project.

Note from the Developer

This is my first attempt at building a project like this, and it has been a fantastic learning experience. While I haven’t worked on storage services before, diving into this project has significantly enhanced my understanding of file management and backend services.

Throughout the development process, I applied clean code principles to ensure the codebase is maintainable and easy to understand. Additionally, I incorporated Factory and Strategy design patterns to handle different storage types effectively, providing flexibility for future enhancements.

To enhance security, I integrated JWT authentication, allowing secure and controlled access to the API.

Your feedback is highly appreciated as I continue to grow and improve my skills in software development.
