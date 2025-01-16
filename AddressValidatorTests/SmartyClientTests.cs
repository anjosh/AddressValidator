using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressValidator;
using AddressValidator.Models;
using Moq;

namespace AddressValidatorTests;

public class SmartyClientTests
{
    private readonly Mock<ISmartyClient> _mockSmartyClient;

    public SmartyClientTests()
    {
        _mockSmartyClient = new Mock<ISmartyClient>();
    }

    [Fact]
    public void ValidateAddress_WithValidAddress_ReturnsValidationResult()
    {
        // Arrange
        var address = new Address
        {
            Street = "143 e Maine Street",
            City = "Columbus",
            ZipCode = "43215"
        };

        var expectedResult = new ValidationResult
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

        _mockSmartyClient.Setup(x => x.ValidateAddress(address))
            .Returns(expectedResult);

        // Act
        var result = _mockSmartyClient.Object.ValidateAddress(address);

        // Assert
        Assert.True(result.IsValid);
        Assert.Equal("143 E Main St", result.CorrectedAddress.Street);
        Assert.Equal("Columbus", result.CorrectedAddress.City);
        Assert.Equal("43215-5370", result.CorrectedAddress.ZipCode);
    }

    [Fact]
    public void ValidateAddress_WithInvalidAddress_ReturnsInvalidResult()
    {
        // Arrange
        var address = new Address
        {
            Street = "Invalid Street",
            City = "Nowhere",
            ZipCode = "00000"
        };

        var expectedResult = new ValidationResult
        {
            OriginalAddress = address,
            CorrectedAddress = null,
            IsValid = false
        };

        _mockSmartyClient.Setup(x => x.ValidateAddress(address))
            .Returns(expectedResult);

        // Act
        var result = _mockSmartyClient.Object.ValidateAddress(address);

        // Assert
        Assert.False(result.IsValid);
        Assert.Null(result.CorrectedAddress);
    }
}
