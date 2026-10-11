using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace ProCredit.BookStore.Auth;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
    [
        new ApiScope("books.crud", "Book CRUD access"),
        new ApiScope("books.search", "Book search access")
    ];

    public static IEnumerable<Client> Clients =>
    [
        new Client
        {
            ClientId = "bookstore-client",
            ClientName = "BookStore Test Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("bookstore-secret".Sha256())
            },
            AllowedScopes =
            {
                "books.crud",
                "books.search"
            },
             AllowedCorsOrigins =
            {
                "http://localhost:5291",
                "https://localhost:7032"
            }
        }
    ];
}