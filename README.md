# CoolShop

Barebones project showcasing usage of ASP NET Core REST API with Entity Framework Core build on .NET6.

# Getting Started

## SDK
.NET6 https://dotnet.microsoft.com/en-us/download/visual-studio-sdks

## Database
Install dotnet ef
```bash
dotnet tool install --global dotnet-ef
```
Apply all available migrations to create local database (MSSQL)
```bash
dotnet ef database update --project src/CoolShop.WebApi
```

## How to run
### API
```bash
dotnet run --project src/CoolShop.WebApi
```
### Tests
```bash
dotnet test
```