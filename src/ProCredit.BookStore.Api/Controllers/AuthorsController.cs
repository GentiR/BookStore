using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProCredit.BookStore.Application.DTOs;
using ProCredit.BookStore.Application.Interfaces.Repositories;
using ProCredit.BookStore.Application.Requests;
using ProCredit.BookStore.Application.Results;
using ProCredit.BookStore.Application.Services;
using ProCredit.BookStore.Application.Validations;
using ProCredit.BookStore.Domain;
using System.Net;

namespace ProCredit.BookStore.Api.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController(IAuthorRepository authorRepository, 
                               IValidator<CreateAuthorRequest> createAuthorValidator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAuthors(CancellationToken cancellationToken)
    {
        var authors = await authorRepository.GetAuthors(cancellationToken);

        var response = authors.Select(author => new AuthorDTO
        {
            AuthorId = author.AuthorId,
            Name = author.Name
        });

        return Ok(response);
    }

    [HttpGet("{authorId:int}")]
    public async Task<IActionResult> GetAuthorById(int authorId, CancellationToken cancellationToken)
    {

        if (authorId <= 0)
        {
            return BadRequest("AuthorId must be greater than 0.");
        }

        var author = await authorRepository.GetAuthorById(authorId, cancellationToken);

        if (author is null)
        {
            return NotFound("Author was not found.");
        }

        return Ok(new AuthorDTO
        {
            AuthorId = author.AuthorId,
            Name = author.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAuthor(CreateAuthorRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await createAuthorValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var author = new Author
        {
            Name = request.Name
        };

        var createdAuthor = await authorRepository.AddAuthor(author, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new AuthorDTO
        {
            AuthorId = createdAuthor.AuthorId,
            Name = createdAuthor.Name
        });
    }
}