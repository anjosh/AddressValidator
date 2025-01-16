using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressValidator;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: AddressValidator <input-file>");
            return;
        }

        var services = ConfigureServices();
        var validator = services.GetRequiredService<IAddressValidator>();

        try
        {
            var results = validator.ValidateAddresses(args[0]);
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services.AddSingleton(configuration);
        services.AddSingleton<IAddressParser, CsvAddressParser>();
        services.AddSingleton<ISmartyClient, SmartyClient>();
        services.AddSingleton<IAddressValidator, AddressValidator>();

        return services.BuildServiceProvider();
    }
}
