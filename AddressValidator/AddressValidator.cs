using AddressValidator.Models;

namespace AddressValidator;

public class AddressValidator : IAddressValidator
{
    private readonly IAddressParser _addressParser;
    private readonly ISmartyClient _smartyService;

    public AddressValidator(
        IAddressParser addressParser,
        ISmartyClient smartyService)
    {
        _addressParser = addressParser ?? throw new ArgumentNullException(nameof(addressParser));
        _smartyService = smartyService ?? throw new ArgumentNullException(nameof(smartyService));
    }

    public IEnumerable<ValidationResult> ValidateAddresses(string inputPath)
    {
        var addresses = _addressParser.ParseAddresses(inputPath);
        var validatedAddresses = addresses.Select(_smartyService.ValidateAddress);

        return validatedAddresses;
    }
}

