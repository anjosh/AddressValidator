using AddressValidator.Models;

namespace AddressValidator
{
    public interface ISmartyClient
    {
        ValidationResult ValidateAddress(Address address);
    }
}