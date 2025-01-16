using Moq;
using Xunit;
using AddressValidator;
using AddressValidator.Models;

namespace AddressValidatorTests;

public class AddressValidatorTests
{
    private readonly Mock<IAddressParser> _mockParser;
    private readonly Mock<ISmartyClient> _mockSmartyClient;
    private readonly IAddressValidator _validator;

    public AddressValidatorTests()
    {
        _mockParser = new Mock<IAddressParser>();
        _mockSmartyClient = new Mock<ISmartyClient>();
        _validator = new AddressValidator.AddressValidator(_mockParser.Object, _mockSmartyClient.Object);
    }

    [Fact]
    public void ValidateAddresses_WithValidAddress_ReturnsValidationResult()
    {
        // Arrange
        var address = new Address
        {
            Street = "143 e Maine Street",
            City = "Columbus",
            ZipCode = "43215"
        };

        var validationResult = new ValidationResult
        {
            OriginalAddress = address,
            CorrectedAddress = new Address
            {
                Street = "143 E Main St",
                City = "Columbus",
                ZipCode = "43215-5370"
            },
            IsValid = true
        };

        _mockParser.Setup(p => p.ParseAddresses(It.IsAny<string>()))
            .Returns(new[] { address });

        _mockSmartyClient.Setup(s => s.ValidateAddress(It.IsAny<Address>()))
            .Returns(validationResult);

        // Act
        var results = _validator.ValidateAddresses("test.csv").ToList();

        // Assert
        Assert.Single(results);
        var result = results.First();
        Assert.True(result.IsValid);
        Assert.Equal("143 E Main St", result.CorrectedAddress.Street);
    }

    [Fact]
    public void ValidateAddresses_WithInvalidAddress_ReturnsInvalidResult()
    {
        // Arrange
        var address = new Address
        {
            Street = "Invalid Street",
            City = "Nowhere",
            ZipCode = "00000"
        };

        var validationResult = new ValidationResult
        {
            OriginalAddress = address,
            CorrectedAddress = null,
            IsValid = false
        };

        _mockParser.Setup(p => p.ParseAddresses(It.IsAny<string>()))
            .Returns(new[] { address });

        _mockSmartyClient.Setup(s => s.ValidateAddress(It.IsAny<Address>()))
            .Returns(validationResult);

        // Act
        var results = _validator.ValidateAddresses("test.csv").ToList();

        // Assert
        Assert.Single(results);
        var result = results.First();
        Assert.False(result.IsValid);
        Assert.Null(result.CorrectedAddress);
    }

    [Fact]
    public void ValidateAddresses_WithMultipleAddresses_ReturnsMultipleResults()
    {
        // Arrange
        var addresses = new[]
        {
            new Address { Street = "143 e Maine Street", City = "Columbus", ZipCode = "43215" },
            new Address { Street = "Invalid Street", City = "Nowhere", ZipCode = "00000" }
        };

        _mockParser.Setup(p => p.ParseAddresses(It.IsAny<string>()))
            .Returns(addresses);

        _mockSmartyClient.Setup(s => s.ValidateAddress(It.Is<Address>(a => a.Street == "143 e Maine Street")))
            .Returns(new ValidationResult
            {
                OriginalAddress = addresses[0],
                CorrectedAddress = new Address
                {
                    Street = "143 E Main St",
                    City = "Columbus",
                    ZipCode = "43215-5370"
                },
                IsValid = true
            });

        _mockSmartyClient.Setup(s => s.ValidateAddress(It.Is<Address>(a => a.Street == "Invalid Street")))
            .Returns(new ValidationResult
            {
                OriginalAddress = addresses[1],
                CorrectedAddress = null,
                IsValid = false
            });

        // Act
        var results = _validator.ValidateAddresses("test.csv").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.True(results[0].IsValid);
        Assert.False(results[1].IsValid);
    }

    [Fact]
    public void ValidateAddresses_WithEmptyFile_ReturnsEmptyResults()
    {
        // Arrange
        _mockParser.Setup(p => p.ParseAddresses(It.IsAny<string>()))
            .Returns(Enumerable.Empty<Address>());

        // Act
        var results = _validator.ValidateAddresses("empty.csv").ToList();

        // Assert
        Assert.Empty(results);
    }
}
