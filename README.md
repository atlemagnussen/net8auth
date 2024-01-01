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


https://authdev.digilean.tools/connect/authorize
?client_id=webclient
&redirect_uri=https%3A%2F%2Fdemo.digilean.tools%2Fcallback.html
&response_type=code
&scope=openid%20profile%20api.read
&state=1ef9232a5e4e431c919a9bedb73ecd79
&code_challenge=dOlA7YXy9MehrFp4KlB8y-gsnOOgCbb8ZTHH1U13qBI
&code_challenge_method=S256
&response_mode=query

https://demo.digilean.tools/callback.html
?code=4596150F94C854C10748978474F63B2CC3E75803CF4AFE6C4599D1A2F69116F0
&scope=openid%20profile%20api.read
&state=1ef9232a5e4e431c919a9bedb73ecd79
&session_state=OHu4xQHR4MfNayWX7kSr_49mQb8OTl46Cc0dT-AAln0.3461E50A6269CA1381112ADA139B5573