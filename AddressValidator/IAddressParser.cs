using AddressValidator.Models;

namespace AddressValidator;

public interface IAddressParser
{
    IEnumerable<Address> ParseAddresses(string inputPath);
}