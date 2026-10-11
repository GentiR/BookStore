# BookStore

This is a REST API for managing books and authors.

The project includes:

- Book CRUD
- Book search by title and author
- Pagination
- Validation
- OAuth2/JWT authorization
- Swagger
- SQL Server
- Docker support

## Projects

- `ProCredit.BookStore.Api` - Web API, controllers, Swagger and authentication setup
- `ProCredit.BookStore.Auth` - OAuth2 provider using Duende IdentityServer
- `ProCredit.BookStore.Domain` - Domain models: `Author` and `Book`
- `ProCredit.BookStore.Application` - Requests, DTOs, validations, services and interfaces
- `ProCredit.BookStore.Infrastructure` - DbContext, repositories and migrations
- `ProCredit.BookStore.Tests` - Test project

## Run with Visual Studio

Open the solution in Visual Studio.

Set these two projects as startup projects:

```text
ProCredit.BookStore.Auth
ProCredit.BookStore.Api

Run the solution.
Swagger opens at: https://localhost:7032/swagger or: http://localhost:5291/swagger

Run with Docker
Make sure Docker Desktop is running.
From the solution folder run:
docker compose up --build

Swagger will be available at:
http://localhost:5291/swagger

Swagger Authorization
  Click Authorize in Swagger.
Use:
  client_id: bookstore-client
  client_secret: bookstore-secret
Select both scopes:
  books.crud
  books.search

Example Test
  Create an author first:
  {
    "authorId": 1,
    "name": "Gent Rechica"
  }
  Then create a book:
  {
    "authorId": 1,
    "title": "First Book",
    "subTitle": "Testing"
  }
  Search books:
  GET /api/books/search?Page=1&PageSize=10


Notes
The API creates the database automatically by applying EF Core migrations on startup.
Docker starts the API, Auth server and SQL Server together.
