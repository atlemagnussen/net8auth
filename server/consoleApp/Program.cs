﻿// See https://aka.ms/new-console-template for more information
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
services.AddAuthenticationServices();
services.AddOptionsConfiguration(configuration);
services.AddSingleton(configuration);

var sp = services.BuildServiceProvider();

// var getCryptoKeys = sp.GetService<GetCryptoKeys>()!;
// CryptoKeys keys = getCryptoKeys.FromConfig();
// Console.WriteLine(keys.ToString());

var cryptoKeyOptions = sp.GetService<IOptions<CryptoKeys>>();
if (cryptoKeyOptions == null || cryptoKeyOptions.Value == null)
{
    Console.WriteLine("missing keys");
    return;
}

CryptoKeys cryptoKeys = cryptoKeyOptions.Value;

var jwtUnsigned = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhdGxlbWFnbnVzc2VuQGdtYWlsLmNvbSIsIm5hbWUiOiJhdGxlIiwicm9sZSI6IkFkbWluIiwiaXNzIjoidGVzdCIsImV4cCI6MTcwNDA5ODM5NywiaWF0IjoxNzA0MTg0Nzk3LCJuYmYiOjE3MDQxODQ3OTd9";

var signature = CryptoService.SignJwk(jwtUnsigned, cryptoKeys);

//Console.WriteLine($"{jwtUnsigned}.{signature}");

//var testEcd = CryptoService.CreateEcKey();
//var serEcd = JsonSerializer.Serialize(testEcd);
//Console.WriteLine(serEcd);

// CryptoKeyPair keyPairJwk = CryptoService.CreateKey();

// Console.WriteLine("private jwk");
// var serPrivat = JsonSerializer.Serialize(keyPairJwk.PrivateKey);
// Console.WriteLine(serPrivat);

// Console.WriteLine("public jwk");
// var serPublic = JsonSerializer.Serialize(keyPairJwk.PublicKey);
// Console.WriteLine(serPublic);