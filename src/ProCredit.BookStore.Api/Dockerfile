FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/ProCredit.BookStore.Api/ProCredit.BookStore.Api.csproj", "src/ProCredit.BookStore.Api/"]
COPY ["src/ProCredit.BookStore.Application/ProCredit.BookStore.Application.csproj", "src/ProCredit.BookStore.Application/"]
COPY ["src/ProCredit.BookStore.Domain/ProCredit.BookStore.Domain.csproj", "src/ProCredit.BookStore.Domain/"]
COPY ["src/ProCredit.BookStore.Infrastructure/ProCredit.BookStore.Infrastructure.csproj", "src/ProCredit.BookStore.Infrastructure/"]

RUN dotnet restore "src/ProCredit.BookStore.Api/ProCredit.BookStore.Api.csproj"

COPY . .
WORKDIR "/src/src/ProCredit.BookStore.Api"
RUN dotnet publish "ProCredit.BookStore.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProCredit.BookStore.Api.dll"]