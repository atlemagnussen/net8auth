// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using net8auth.consoleApp;
using net8auth.model;
using net8auth.model.Test;

Console.WriteLine("Hello, World!");
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAuthenticationServices();
builder.Services.AddOptionsConfiguration(builder.Configuration);

var host = builder.Build();

var getCryptoKeys = host.Services.GetRequiredService<GetCryptoKeys>();

CryptoKeys keys = getCryptoKeys.FromConfig();
Console.WriteLine(keys.ToString());

//await host.RunAsync();

// var tokenService = sp.GetService<ITokenService>()!;
// var jwt = await tokenService.CreateAndSignJwt(new ClaimsPrincipal());
// Console.WriteLine("jwt");
// Console.WriteLine(jwt);

// var key = CreateKeys.CreateEcKey();
// var keyStr = JsonSerializer.Serialize(key);
// Console.WriteLine(keyStr);
