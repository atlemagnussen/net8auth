// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using net8auth.consoleApp;
using net8auth.model;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder();
IConfiguration configuration = builder.GetConfigCmd(args);

var services = new ServiceCollection();
services.AddOptions();

services.AddOptionsConfiguration(configuration);

var sp = services.BuildServiceProvider();

var keyPairOptions = sp.GetService<IOptions<CryptoKeyPair>>();
if (keyPairOptions == null)
    return;
var keyPair = keyPairOptions.Value;

var jwtUnsigned = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdGxlbWFnbnVzc2VuQGdtYWlsLmNvbSIsIm5hbWUiOiJhdGxlIiwicm9sZSI6IkFkbWluIiwiaXNzIjoidGVzdCIsImV4cCI6MTcwNDA5ODM5NywiaWF0IjoxNzA0MTg0Nzk3LCJuYmYiOjE3MDQxODQ3OTd9";

var signature = CryptoService.SignJwk(jwtUnsigned, keyPair);

Console.WriteLine($"{jwtUnsigned}.{signature}");

// CryptoKeyPair keyPairJwk = CryptoService.CreateKey();

// Console.WriteLine("private jwk");
// var serPrivat = JsonSerializer.Serialize(keyPairJwk.PrivateKey);
// Console.WriteLine(serPrivat);

// Console.WriteLine("public jwk");
// var serPublic = JsonSerializer.Serialize(keyPairJwk.PublicKey);
// Console.WriteLine(serPublic);