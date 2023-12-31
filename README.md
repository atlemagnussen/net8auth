# try auth with net8

https://devblogs.microsoft.com/dotnet/whats-new-with-identity-in-dotnet-8/

https://learn.microsoft.com/en-us/dotnet/core/compatibility/aspnet-core/8.0/securitytoken-events


## Migrations

Usual stuff, install or update dotnet-ef

```sh
dotnet tool install --global dotnet-ef

dotnet tool update --global dotnet-ef
```

then

```sh
dotnet ef migrations add UpdatedUser

dotnet ef database update
```

## templates used

Auth
```sh
dotnet new webapp --auth Individual --use-program-main
````

Api
```sh
dotnet new webapi --use-program-main --use-controllers
```