using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ProCredit.BookStore.Application.Interfaces.Repositories;
using ProCredit.BookStore.Application.Interfaces.Services;
using ProCredit.BookStore.Application.Services;
using ProCredit.BookStore.Application.Validations;
using ProCredit.BookStore.Infrastructure.DbContexts;
using ProCredit.BookStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BookStoreDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });
});

var authority = builder.Configuration["Authentication:Authority"] ?? "http://localhost:5019";
var validIssuer = builder.Configuration["Authentication:ValidIssuer"] ?? authority;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.ValidIssuer = validIssuer;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("books.crud", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "books.crud");
    });

    options.AddPolicy("books.search", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "books.search");
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("http://localhost:5019/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    ["books.crud"] = "Book CRUD access",
                    ["books.search"] = "Book search access"
                }
            }
        }
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("oauth2", document)] =
        [
            "books.crud",
            "books.search"
        ]
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("bookstore-client");
        options.OAuthClientSecret("bookstore-secret");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();