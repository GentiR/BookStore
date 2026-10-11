FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/ProCredit.BookStore.Auth/ProCredit.BookStore.Auth.csproj", "src/ProCredit.BookStore.Auth/"]

RUN dotnet restore "src/ProCredit.BookStore.Auth/ProCredit.BookStore.Auth.csproj"

COPY . .
WORKDIR "/src/src/ProCredit.BookStore.Auth"
RUN dotnet publish "ProCredit.BookStore.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProCredit.BookStore.Auth.dll"]