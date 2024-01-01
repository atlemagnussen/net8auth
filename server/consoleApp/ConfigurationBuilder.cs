using Microsoft.Extensions.Configuration;

namespace net8auth.consoleApp;

public static class ConfigurationBuilderExtension
{
    public static IConfigurationRoot GetConfigCmd(this IConfigurationBuilder builder, string[] args)
        {
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(prefix: "ConfigPrefix_")
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args);

            var configuration = builder.Build();
            return configuration;
        }
}