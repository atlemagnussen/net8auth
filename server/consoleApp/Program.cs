// See https://aka.ms/new-console-template for more information
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using net8auth.consoleApp;
using net8auth.model;
using net8auth.model.Test;
using net8auth.model.Tokens;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder();
IConfiguration configuration = builder.GetConfigCmd(args);

var services = new ServiceCollection();
services.AddAuthenticationServices();
services.AddOptionsConfiguration(configuration);
services.AddSingleton(configuration);

var sp = services.BuildServiceProvider();

// var getCryptoKeys = sp.GetService<GetCryptoKeys>()!;
// CryptoKeys keys = getCryptoKeys.FromConfig();
// Console.WriteLine(keys.ToString());

// var tokenService = sp.GetService<ITokenService>()!;
// var jwt = await tokenService.CreateAndSignJwt(new ClaimsPrincipal());
// Console.WriteLine("jwt");
// Console.WriteLine(jwt);

var key = CreateKeys.CreateEcKey();
var keyStr = JsonSerializer.Serialize(key);
Console.WriteLine(keyStr);
