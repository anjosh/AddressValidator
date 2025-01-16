using AddressValidator;
using Xunit;

namespace AddressValidatorTests;

public class CsvAddressParserTests
{
    private readonly string _testFilePath = "test.csv";
    private readonly CsvAddressParser _parser;

    public CsvAddressParserTests()
    {
        _parser = new CsvAddressParser();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ParseAddresses_WithInvalidFilePath_ThrowsArgumentException(string path)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _parser.ParseAddresses(path));
        Assert.Contains("Input path cannot be null or empty", exception.Message);
    }

    [Fact]
    public void ParseAddresses_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() =>
            _parser.ParseAddresses("nonexistent.csv"));
        Assert.Contains("Input file not found", exception.Message);
    }

    [Fact]
    public void ParseAddresses_WithValidCsv_ReturnsAddresses()
    {
        // Arrange
        var csvContent = @"Street,City,Zip Code
143 e Maine Street,Columbus,43215
1 Empora St,Title,11111";

        File.WriteAllText(_testFilePath, csvContent);

        try
        {
            // Act
            var addresses = _parser.ParseAddresses(_testFilePath).ToList();

            // Assert
            Assert.Equal(2, addresses.Count);

            Assert.Equal("143 e Maine Street", addresses[0].Street);
            Assert.Equal("Columbus", addresses[0].City);
            Assert.Equal("43215", addresses[0].ZipCode);

            Assert.Equal("1 Empora St", addresses[1].Street);
            Assert.Equal("Title", addresses[1].City);
            Assert.Equal("11111", addresses[1].ZipCode);
        }
        finally
        {
            File.Delete(_testFilePath);
        }
    }

    [Fact]
    public void ParseAddresses_WithEmptyCsv_ReturnsEmptyList()
    {
        // Arrange
        var csvContent = "Street,City,Zip Code";
        File.WriteAllText(_testFilePath, csvContent);

        try
        {
            // Act
            var addresses = _parser.ParseAddresses(_testFilePath).ToList();

            // Assert
            Assert.Empty(addresses);
        }
        finally
        {
            File.Delete(_testFilePath);
        }
    }

    [Fact]
    public void ParseAddresses_WithMalformedCsv_ThrowsCsvHelperException()
    {
        // Arrange
        var csvContent = @"Street,City
143 e Maine Street,Columbus,43215"; // Missing header column

        File.WriteAllText(_testFilePath, csvContent);

        try
        {
            // Act & Assert
            Assert.Throws<CsvHelper.HeaderValidationException>(() =>
                _parser.ParseAddresses(_testFilePath).ToList());
        }
        finally
        {
            File.Delete(_testFilePath);
        }
    }
}
