using AddressValidator.Models;

namespace AddressValidator
{
    public interface IAddressValidator
    {
        IEnumerable<ValidationResult> ValidateAddresses(string inputPath);
    }
}