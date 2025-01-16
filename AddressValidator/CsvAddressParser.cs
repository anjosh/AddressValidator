using System.Globalization;
using System.Text;
using AddressValidator.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AddressValidator;

public class CsvAddressParser : IAddressParser
{
    public IEnumerable<Address> ParseAddresses(string inputPath)
    {
        if (string.IsNullOrWhiteSpace(inputPath))
        {
            throw new ArgumentException("Input path cannot be null or empty", nameof(inputPath));
        }

        if (!File.Exists(inputPath))
        {
            throw new FileNotFoundException("Input file not found", inputPath);
        }

        using var reader = new StreamReader(inputPath);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.Trim(),
            TrimOptions = TrimOptions.Trim,
        };

        using var csv = new CsvReader(reader, config);

        return csv.GetRecords<Address>().ToList();
    }
}
