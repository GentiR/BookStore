# Project Structure

This document explains how the solution is organized and why the projects are separated.

## Main Flow

The API flow is:

```text
Request -> Controller -> Validator -> Service -> Repository -> Database

# Domain Project:
  ProCredit.BookStore.Domain
    This project contains the main models:
      Author
      Book

These models represent the data that is stored in the database.
The Book model stores AuthorId, because in the database the book is connected to the author through a foreign key.


# Application Project:
  ProCredit.BookStore.Application
    This project contains:
      Requests
      DTOs
      Validations
      Services
      Interfaces
      Results
      ViewModels
      Requests
      Requests are used for data that comes from the client to the API.
      Examples:
        CreateBookRequest
        UpdateBookRequest
        SearchBooksRequest

For creating a book, I send only AuthorId, not the full author object, because the API only needs to know which author the book belongs to.

DTOs
  DTOs are used for data returned from the API.
  Examples:
    BookDTO
    AuthorDTO
    PagedResultDTO
  The BookDTO returns the author object together with the book, because the required object model says that a book has an author.

Validations
  Validation is done with FluentValidation.
  Examples:
    CreateBookRequestValidator
    UpdateBookRequestValidator
    SearchBooksRequestValidator
  This keeps the controller cleaner and makes the validation easier to maintain.

Services
  Services contain the business logic.
  For example, when creating or updating a book, the service checks if the author exists.
  If the author does not exist, the API returns:
    Author was not found.
  This is better than allowing the database to throw a foreign key error.

Repository Interfaces
  Repository interfaces are placed in the Application project.
  The service depends on interfaces, not directly on Entity Framework or SQL Server.
  Examples:
    IBookRepository
    IAuthorRepository

# Infrastructure Project:
  ProCredit.BookStore.Infrastructure
    This project contains the database implementation.
    It contains:
      BookStoreDbContext
      BookStoreDbContext defines:
        Tables
        Primary keys
        Foreign keys
        Relationships
        Column limits
        Schema
      Repositories
      Migrations
      DbContext

  The database schema is:
  bookstore
    Tables:
    bookstore.Authors
    bookstore.Books

  Repositories
    Repositories are used only for communication with the database.
    For example:
      Add book
      Update book
      Delete book
      Search books
      Get authors
      For search and get book by id, the repository joins books with authors and returns a read model/view model.
      Example:
      BookWithAuthorViewModel
  This is useful because the database stores AuthorId, but the API response needs the author details too.


# API Project:
    ProCredit.BookStore.Api
      This project contains:
        Controllers
        Program.cs
        Swagger setup
        Authentication setup
        Authorization policies
        Controllers receive requests, validate them, call the service, and return the correct HTTP response.
        The controller does not contain database logic.


# Auth Project:
    ProCredit.BookStore.Auth
    This project uses Duende IdentityServer as the OAuth2 provider.
    It creates JWT tokens for the API.

    Configured client:
      client_id: bookstore-client
      client_secret: bookstore-secret

    Configured scopes:
      books.crud
      books.search
      books.crud is used for create, update, and delete book operations.
      books.search is used for search and get book by id.

    Docker
      Docker is used so the project can run with minimum setup.
      Docker starts:
      SQL Server
      Auth server
      API
    Files used:
      docker-compose.yml
      src/ProCredit.BookStore.Api/Dockerfile
      src/ProCredit.BookStore.Auth/Dockerfile
      The API applies migrations automatically on startup, so the database is created inside the SQL Server container without manual setup.
