// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using net8auth.consoleApp;

Console.WriteLine("Hello, World!");

var builder = new ConfigurationBuilder();
IConfiguration configuration = builder.GetConfigCmd(args);

CryptoKeyPair keyPair = new CryptoKeyPair();
var sectionKey = configuration.GetSection("CryptoKey");
sectionKey.Bind(keyPair);

var services = new ServiceCollection();
services.AddOptions();

