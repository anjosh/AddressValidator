# Address Validator

A command-line tool that validates US addresses against the Smarty API and outputs either the corrected address or indicates an invalid address.

## Prerequisites

- .NET 8.0 SDK
- A Smarty API key

## Setup

1. Clone the repository:
```
git clone <repository-url>
cd AddressValidator
```

2. Install the required NuGet packages:
```
dotnet restore
```

3. Configure your Smarty API key:
   - Open `appsettings.json`
   - Replace `SMARTY_AUTH_ID` and `SMARTY_AUTH_TOKEN` with your own auth id and token

## Building

```
dotnet build
```

## Running

Run the application:
```
dotnet run <input-file.csv>
```

## Running Tests

```
cd ../AddressValidatorTests
dotnet test
```

## Design Decisions

1. **Single Responsibility Principle**: Each class has a single, well-defined purpose:
   - `AddressValidator`: Orchestrates the validation process
   - `SmartyClient`: Handles using the Smarty SDK to validate the address
   - `CsvAddressParser`: Manages CSV file parsing

2. **Dependency Injection**: Used throughout the application for:
   - Loose coupling between components
   - Easier unit testing through mocking

## Potential Improvements

1. Add batch processing for large files
2. Move away from using the SDK since it doesn't have async methods
3. Add integration or end-to-end testing
