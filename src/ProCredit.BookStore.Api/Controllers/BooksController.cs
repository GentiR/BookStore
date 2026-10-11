using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProCredit.BookStore.Application.Interfaces.Services;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Application.Results;

namespace ProCredit.BookStore.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(
    IBookService bookService,
    IValidator<CreateBookRequest> createBookValidator,
    IValidator<UpdateBookRequest> updateBookValidator,
    IValidator<SearchBooksRequest> searchBooksValidator) : ControllerBase
{

    [Authorize(Policy = "books.search")]
    [HttpGet("{bookId:int}")]
    public async Task<IActionResult> GetBookWithAuthorById(int bookId, CancellationToken cancellationToken)
    {

        if (bookId <= 0)
        {
            return BadRequest("BookId must be greater than 0.");
        }

        var book = await bookService.GetBookWithAuthorById(bookId, cancellationToken);

        if (book is null)
        {
            return NotFound("Book was not found.");
        }

        return Ok(book);
    }

    [Authorize(Policy = "books.crud")]
    [HttpPost]
    public async Task<IActionResult> AddBook(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await createBookValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await bookService.AddBook(request, cancellationToken);

        if (result is BookOperationResult.AuthorNotFound)
        {
            return NotFound("Author was not found.");
        }

        return StatusCode(StatusCodes.Status201Created, "Book was successfully added.");
    }

    [Authorize(Policy = "books.crud")]
    [HttpPut]
    public async Task<IActionResult> UpdateBook(UpdateBookRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await updateBookValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await bookService.UpdateBook(request, cancellationToken);

        if (result is BookOperationResult.AuthorNotFound)
        {
            return NotFound("Author was not found.");
        }

        if (result is BookOperationResult.BookNotFound)
        {
            return NotFound("Book was not found.");
        }

        return Ok("Book was successfully updated.");
    }

    [Authorize(Policy = "books.crud")]
    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> DeleteBook(int bookId, CancellationToken cancellationToken)
    {

        if (bookId <= 0)
        {
            return BadRequest("BookId must be greater than 0.");
        }

        var deleted = await bookService.DeleteBook(bookId, cancellationToken);

        if (!deleted)
        {
            return NotFound("Book was not found.");
        }

        return Ok("Book was successfully deleted.");
    }

    [Authorize(Policy = "books.search")]
    [HttpGet("search")]
    public async Task<IActionResult> GetBooks([FromQuery] SearchBooksRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await searchBooksValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var books = await bookService.GetBooks(request, cancellationToken);

        return Ok(books);
    }
}
