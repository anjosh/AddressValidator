using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressValidator.Models;

public class ValidationResult
{
    public Address OriginalAddress { get; set; }
    public Address? CorrectedAddress { get; set; }
    public bool IsValid { get; set; }

    public override string ToString()
    {
        return IsValid
            ? $"{OriginalAddress} -> {CorrectedAddress}"
            : $"{OriginalAddress} -> Invalid Address";
    }
}
