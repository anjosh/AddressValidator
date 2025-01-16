using CsvHelper.Configuration.Attributes;

namespace AddressValidator.Models
{
    public class Address
    {
        [Name("Street")]
        public string Street { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("Zip Code")]
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return $"{Street}, {City}, {ZipCode}";
        }
    }
}