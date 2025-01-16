using System.Net.Http.Json;
using AddressValidator.Models;
using Microsoft.Extensions.Configuration;
using SmartyStreets;
using SmartyStreets.USStreetApi;

namespace AddressValidator;

public class SmartyClient : ISmartyClient
{
    private readonly IConfiguration _configuration;

    public SmartyClient(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public ValidationResult ValidateAddress(Address address)
    {
        var authId = _configuration[Constants.SMARTY_AUTH_ID_KEY];
        var authToken = _configuration[Constants.SMARTY_AUTH_TOKEN_KEY];

        var client = new ClientBuilder(authId, authToken)
            .WithLicense([Constants.SMARTY_LICENSE_US_CORE_CLOUD])
            .BuildUsStreetApiClient();

        var lookup = new Lookup
        {
            Street = address.Street,
            City = address.City,
            ZipCode = address.ZipCode
        };

        client.Send(lookup);

        var candidates = lookup.Result;

        if (candidates.Count == 0)
        {
            return new ValidationResult
            {
                OriginalAddress = address,
                IsValid = false
            };
        }

        var validatedAddress = candidates[0];
        return new ValidationResult
        {
            OriginalAddress = address,
            CorrectedAddress = new Address
            {
                Street = validatedAddress.DeliveryLine1,
                City = validatedAddress.Components.CityName,
                ZipCode = $"{validatedAddress.Components.ZipCode}-{validatedAddress.Components.Plus4Code}"
            },
            IsValid = true
        };
    }
}
