# SGuard.DataAnnotations

![CI](https://github.com/selcukgural/SGuard.DataAnnotations/actions/workflows/ci.yml/badge.svg)
[![NuGet Version](https://img.shields.io/nuget/v/SGuard.DataAnnotations.svg?style=flat-square)](https://www.nuget.org/packages/SGuard.DataAnnotations/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/SGuard.DataAnnotations.svg?style=flat-square)](https://www.nuget.org/packages/SGuard.DataAnnotations/)
[![Coverage](https://codecov.io/gh/selcukgural/SGuard.DataAnnotations/branch/main/graph/badge.svg)](https://codecov.io/gh/selcukgural/SGuard.DataAnnotations)


**SGuard.DataAnnotations** provides localized and extensible DataAnnotations support for .NET, including:
- Localizable validation attributes (with robust fallback and custom error handling)
- Collection, conditional, and property comparison validators are not found in standard DataAnnotations
- Guard pattern (`Is.*` for boolean return, `ThrowIf.*` for exception-throwing) for model validation
- Seamless integration with DataAnnotations and SGuard's fail-fast/callback philosophy
- Well-tested and extensible for real-world application scenarios

> **Note:** For fluent validation/guard support, see the upcoming [`SGuard.FluentValidation`](https://github.com/selcukgural/SGuard.FluentValidation) package.
>
> **Important:** If you want to implement custom callback, fail-fast, or chainable guard logic, you should also review the [SGuard core project](https://github.com/selcukgural/SGuard), which provides advanced guard and callback APIs used by this library.

---

## Table of Contents

- [Installation](#installation)
- [Features](#features)
- [Supported Languages](#supported-languages)
- [Supported Attributes](#supported-attributes)
    - [String & Common Validators](#string--common-validators)
    - [Collection Validators](#collection-validators)
    - [Conditional Validators](#conditional-validators)
    - [Comparison Validators](#comparison-validators)
- [Guard Pattern API](#guard-pattern-api)
    - [Is.* Methods](#is-methods)
    - [ThrowIf.* Methods](#throwif-methods)
- [Localization & Fallback](#localization--fallback)
- [Extending SGuard.DataAnnotations](#extending-sguarddataannotations)
- [Minimal API Example (Real World)](#minimal-api-example-real-world)
- [Advanced Topics](#advanced-topics)
    - [Error Handling](#error-handling)
- [FAQ / Tips](#faq--tips)
- [Contributing](#contributing)
- [License](#license)

---

## Installation

Install via NuGet:

```bash
dotnet add package SGuard.DataAnnotations
```

---

## Features

- **Localized error messages** via resource files (`.resx`), with fallback to default or custom messages.
- **Advanced collection and conditional validation** (min/max count, required-if, required collection, collection element validation, etc.).
- **Comparison attributes** for property-to-property or value-to-value checks (greater than, less than, between, compare, etc.).
- **Full DataAnnotations compatibility**—use SGuard attributes anywhere a standard attribute is accepted.
- **Guard pattern API** (`Is`/`ThrowIf`) for easy imperative validation and exception/callback handling.

---

## Supported Languages

SGuard.DataAnnotations ships with built-in resource support for the following languages:

| Language          | Culture Code | Localized Resource File         |
|-------------------|--------------|---------------------------------|
| English (default) | `en`         | `SGuardDataAnnotations.resx`    |
| Turkish           | `tr`         | `SGuardDataAnnotations.tr.resx` |
| German            | `de`         | `SGuardDataAnnotations.de.resx` |
| French            | `fr`         | `SGuardDataAnnotations.fr.resx` |
| Russian           | `ru`         | `SGuardDataAnnotations.ru.resx` |
| Japanese          | `ja`         | `SGuardDataAnnotations.ja.resx` |
| Hindi             | `hi`         | `SGuardDataAnnotations.hi.resx` |

> **Note:**
> - If the current UI culture is not found, SGuard will fallback to English or to the fallback message if provided.
> - You can add your own resource files to support additional languages.
> - [How to add your own language?](#how-to-add-a-custom-language)
---

## Supported Attributes

### String & Common Validators

| Attribute                          | Purpose                              | Supported Types                  | Example Usage                                                                                                         |
|------------------------------------|--------------------------------------|----------------------------------|-----------------------------------------------------------------------------------------------------------------------|
| `SGuardRequiredAttribute`          | Required field (localized)           | Any                              | `[SGuardRequired(typeof(Resources.SGuardDataAnnotations), "Username_Required")]`                                      |
| `SGuardMinLengthAttribute`         | Minimum string length                | `string`, `array`, `ICollection` | `[SGuardMinLength(5, typeof(Resources.SGuardDataAnnotations), "Username_MinLength")]`                                 |
| `SGuardMaxLengthAttribute`         | Maximum string length                | `string`, `array`, `ICollection` | `[SGuardMaxLength(20, typeof(Resources.SGuardDataAnnotations), "Username_MaxLength")]`                                |
| `SGuardStringLengthAttribute`      | Min/max string length                | `string`                         | `[SGuardStringLength(12, typeof(Resources.SGuardDataAnnotations), "Username_MaxLength")]`                             |
| `SGuardRegularExpressionAttribute` | Regex pattern                        | `string`                         | `[SGuardRegularExpression("^[a-zA-Z0-9_]+$", typeof(Resources.SGuardDataAnnotations), "Username_InvalidCharacters")]` |
| `SGuardEmailAddressAttribute`      | Email format                         | `string`                         | `[SGuardEmailAddress(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat")]`                                |
| `SGuardPhoneAttribute`             | Phone format                         | `string`                         | `[SGuardPhone(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid")]`                                     |
| `SGuardUrlAttribute`               | URL format                           | `string`                         | `[SGuardUrl(typeof(Resources.SGuardDataAnnotations), "Common_Url_Invalid")]`                                          |
| `SGuardCreditCardAttribute`        | Credit card format                   | `string`                         | `[SGuardCreditCard(typeof(Resources.SGuardDataAnnotations), "Common_CreditCard_Invalid")]`                            |
| `SGuardRangeAttribute`             | Value must be within a numeric range | `int`, `double`                  | `[SGuardRange(1, 10, typeof(Resources.SGuardDataAnnotations), "Common_Range")]`                                       |

### Collection Validators

| Attribute                             | Purpose                                                         | Supported Types             | Example Usage                                                                                                                                            |
|---------------------------------------|-----------------------------------------------------------------|-----------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------|
| `SGuardRequiredCollectionAttribute`   | Collection must not be null/empty                               | `IEnumerable`, arrays, etc. | `[SGuardRequiredCollection(typeof(Resources.SGuardDataAnnotations), "Common_Collection_Required")]`                                                      |
| `SGuardMinCountAttribute`             | Minimum item count in collection                                | `IEnumerable`, arrays, etc. | `[SGuardMinCount(2, typeof(Resources.SGuardDataAnnotations), "Common_Collection_MinCount")]`                                                             |
| `SGuardMaxCountAttribute`             | Maximum item count in collection                                | `IEnumerable`, arrays, etc. | `[SGuardMaxCount(5, typeof(Resources.SGuardDataAnnotations), "Common_Collection_MaxCount")]`                                                             |
| `SGuardCollectionItemsMatchAttribute` | Each item must match one/more validators (e.g. regex, required) | `IEnumerable`, arrays, etc. | `[SGuardCollectionItemsMatch(typeof(EmailAddressAttribute), typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat", AggregateAllErrors = true)]` |

**Details:**
- `SGuardCollectionItemsMatchAttribute` can take multiple validators and will apply them to each item.
- `AggregateAllErrors` (default: `false`): If `true`, collects all errors; if `false`, returns on first failure.
- Supported on any `IEnumerable` (e.g., `List<T>`, arrays, custom collections).

### Conditional Validators

| Attribute                   | Purpose                                         | Supported Types | Example Usage                                                                                       |
|-----------------------------|-------------------------------------------------|-----------------|-----------------------------------------------------------------------------------------------------|
| `SGuardRequiredIfAttribute` | Field required if another property equals value | Any             | `[SGuardRequiredIf("Country", "USA", typeof(Resources.SGuardDataAnnotations), "Address_Required")]` |

### Comparison Validators

| Attribute                    | Purpose                                      | Supported Types   | Example Usage                                                                                            |
|------------------------------|----------------------------------------------|-------------------|----------------------------------------------------------------------------------------------------------|
| `SGuardCompareAttribute`     | Values must be equal (like CompareAttribute) | Any               | `[SGuardCompare("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch")]`              |
| `SGuardBetweenAttribute`     | Value must be between two properties         | IComparable types | `[SGuardBetween("Min", "Max", true, typeof(Resources.SGuardDataAnnotations), "Common_Between")]`         |
| `SGuardGreaterThanAttribute` | Value must be greater than another property  | IComparable types | `[SGuardGreaterThan("MinAge", typeof(Resources.SGuardDataAnnotations), "Profile_BirthDate_MinimumAge")]` |
| `SGuardLessThanAttribute`    | Value must be less than another property     | IComparable types | `[SGuardLessThan("MaxAge", typeof(Resources.SGuardDataAnnotations), "Profile_BirthDate_MaximumAge")]`    |

**Supported types:** `int`, `double`, `decimal`, `DateTime`, `string`, etc. (anything implementing `IComparable`)

---

## Guard Pattern API

**SGuard.DataAnnotations** provides two imperative APIs for runtime validation, following the SGuard pattern.

### Is.* Methods

- **Purpose:** Return `bool` for validation checks (never throw).
- **Callback:** Optional `SGuardCallback` invoked with `GuardOutcome.Success`/`Failure`.  
  For advanced callback usage, see the [SGuard project documentation](https://github.com/selcukgural/SGuard).
- **Example:**
    ```csharp
    if (Is.DataAnnotationsValid(model))
    {
        // model is valid
    }
    ```

- **With callback:**
    ```csharp
    bool valid = Is.DataAnnotationsValid(model, callback: outcome =>
    {
        if (outcome == GuardOutcome.Failure)
            Console.WriteLine("Validation failed!");
    });
    ```

- **Get all validation errors:**
    ```csharp
    bool valid = Is.DataAnnotationsValid(model, out var results);
    foreach (var err in results)
        Console.WriteLine($"{string.Join(", ", err.MemberNames)}: {err.ErrorMessage}");
    ```

### ThrowIf.* Methods

- **Purpose:** Throw exception if validation fails.
- **Callback:** Invoked before throw (`GuardOutcome.Failure`) or on pass (`GuardOutcome.Success`).
- **Example:**
    ```csharp
    ThrowIf.DataAnnotationsInValid(model);
    // Throws DataAnnotationsException if model is invalid.
    ```

- **Custom exception:**
    ```csharp
    ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new ArgumentException("Custom error!"));
    ```

- **Custom exception with constructor args:**
    ```csharp
    ThrowIf.DataAnnotationsInValid<ArgumentException>(model, new object[] { "Custom error!" });
    ```

#### Exception Details

- Throws `DataAnnotationsException` by default, which contains all validation errors.
- Extract errors (see also [`SGuard.DataAnnotations.Extensions`](./SGuard.DataAnnotations/src/Extensions/DataAnnotationsExceptionExtensions.cs)):
    ```csharp
    catch (DataAnnotationsException ex)
    {
        if (ex.TryGetValidationErrors(out var errors))
        {
            foreach (var err in errors)
                Console.WriteLine($"{string.Join(", ", err.Members)}: {err.Message}");
        }
    }
    ```

---

## Localization & Fallback

- All SGuard attributes support:
    - `ErrorMessageResourceType` and `ErrorMessageResourceName` (standard .NET resource workflow)
    - `FallbackResourceName`: Used if the main resource key is missing.
    - `FallbackMessage`: Used if both resource keys are missing.
- **Culture:** Error messages are localized to the current `UICulture`.
- **Example:**
    ```csharp
    [SGuardMinLength(3, typeof(Resources.SGuardDataAnnotations), "Username_MinLength", 
        FallbackResourceName = "Common_MinLength", FallbackMessage = "Min length required.")]
    public string Username { get; set; }
    ```

### How to add a custom language?

1. Copy `SGuardDataAnnotations.resx` and rename to e.g. `SGuardDataAnnotations.es.resx` for Spanish.
2. Translate all keys/values.
3. Rebuild and set your thread/UI culture accordingly.

---

## Extending SGuard.DataAnnotations

Want to add your own fully localized validation attribute?  
Inherit from `SGuardValidationAttributeBase` and implement `IsValid`:

```csharp
public class MyCustomLocalizedAttribute : SGuardValidationAttributeBase
{
    public MyCustomLocalizedAttribute(Type resourceType, string resourceName)
        : base(resourceType, resourceName) {}

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Your logic here
        if (value == null) 
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        return ValidationResult.Success;
    }
}
```

---

## Minimal API Example (Real World)

Here's how you use SGuard.DataAnnotations in an ASP.NET Core Minimal API:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SGuard.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/register", (UserRegistration model) =>
{
    if (!Is.DataAnnotationsValid(model, out var errors))
        return Results.BadRequest(errors.Select(e => new { e.MemberNames, e.ErrorMessage }));

    // If valid, continue
    return Results.Ok("Registration successful!");
});

app.Run();

public class UserRegistration
{
    [SGuardRequired(typeof(Resources.SGuardDataAnnotations), "Username_Required")]
    [SGuardMinLength(3, typeof(Resources.SGuardDataAnnotations), "Username_MinLength")]
    [SGuardMaxLength(20, typeof(Resources.SGuardDataAnnotations), "Username_MaxLength")]
    public string Username { get; set; }

    [SGuardRequired(typeof(Resources.SGuardDataAnnotations), "Email_Required")]
    [SGuardEmailAddress(typeof(Resources.SGuardDataAnnotations), "Email_InvalidFormat")]
    public string Email { get; set; }

    [SGuardRequired(typeof(Resources.SGuardDataAnnotations), "Password_Required")]
    [SGuardStringLength(100, typeof(Resources.SGuardDataAnnotations), "Password_MaxLength")]
    public string Password { get; set; }

    [SGuardCompare("Password", typeof(Resources.SGuardDataAnnotations), "Password_Mismatch")]
    public string ConfirmPassword { get; set; }

    [SGuardRequiredCollection(typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Required")]
    [SGuardCollectionItemsMatch(typeof(SGuardPhoneAttribute), typeof(Resources.SGuardDataAnnotations), "Profile_Phone_Invalid", AggregateAllErrors = true)]
    public List<string> PhoneNumbers { get; set; }
}
```

---

## Advanced Topics

### Error Handling

- **Guard methods**: Return `bool` or throw, never both.
- **Attributes**: Always return `ValidationResult`, never throw.
- **All exceptions**: Contain full error detail, member names, and support for extraction/extension.
- **For advanced fail-fast, callback, or custom guard usage:**  
  See [SGuard project documentation](https://github.com/selcukgural/SGuard).

---

## FAQ / Tips

**Q:** _Can I use SGuard attributes in ASP.NET Core, Blazor, WinForms, etc.?_  
**A:** Yes, SGuard attributes implement the standard DataAnnotations contract.

**Q:** _What happens if a resource key is missing?_  
**A:** The attribute will use `FallbackResourceName` if provided, otherwise `FallbackMessage`, otherwise `[ResourceKey]`.

**Q:** _How do I validate a collection’s items?_  
**A:** Use `[SGuardCollectionItemsMatch(...)]`. See [Collection Validators](#collection-validators).

**Q:** _Can I chain SGuard and standard DataAnnotations attributes?_  
**A:** Yes, you can stack any combination on your model.

**Q:** _Will SGuard.DataAnnotations work with FluentValidation?_  
**A:** Yes, as long as you use DataAnnotations integration. For a full fluent API, see [`SGuard.FluentValidation`](https://github.com/selcukgural/SGuard.ValidationBuilder).

**Q:** _How do I quickly test everything?_  
**A:**
1. Run all tests (requires .NET SDK):
    ```bash
    dotnet test
    ```
2. For a quick validation in your app, call:
    ```csharp
    if (!Is.DataAnnotationsValid(model, out var results))
        // handle errors, see 'results'
    ```

---

## Contributing

Pull requests, issues, and suggestions are very welcome!  
See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

---

## License

MIT License. See [LICENSE](LICENSE).

---

## See Also

- [SGuard (core)](https://github.com/selcukgural/SGuard)
- [SGuard.FluentValidation (upcoming)](https://github.com/selcukgural/SGuard.FluentValidation)

---
<!-- TEST-RESULTS:START -->
## Test and Coverage Status

### Test Results

| Total | Passed | Failed | Skipped |
|------:|-------:|-------:|--------:|
| 353 | 353 | 0 | 0 |

### Code Coverage
# Summary
|||
|:---|:---|
| Generated on: | 09/12/2025 - 12:00:34 |
| Coverage date: | 09/09/2025 - 22:27:26 - 09/12/2025 - 12:00:31 |
| Parser: | MultiReport (7x Cobertura) |
| Assemblies: | 1 |
| Classes: | 25 |
| Files: | 25 |
| **Line coverage:** | 87% (276 of 317) |
| Covered lines: | 276 |
| Uncovered lines: | 41 |
| Coverable lines: | 317 |
| Total lines: | 1470 |
| **Branch coverage:** | 80.7% (185 of 229) |
| Covered branches: | 185 |
| Total branches: | 229 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

|**Name**|**Covered**|**Uncovered**|**Coverable**|**Total**|**Line coverage**|**Covered**|**Total**|**Branch coverage**|
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
|**SGuard.DataAnnotations**|**276**|**41**|**317**|**1470**|**87%**|**185**|**229**|**80.7%**|
|SGuard.DataAnnotations.CompareToAttribute|26|13|39|139|66.6%|16|31|51.6%|
|SGuard.DataAnnotations.Exceptions.DataAnnotationsException|14|0|14|62|100%|2|2|100%|
|SGuard.DataAnnotations.Extensions.DataAnnotationsExceptionExtensions|9|1|10|47|90%|9|10|90%|
|SGuard.DataAnnotations.Internal.SGuardDataAnnotations|10|2|12|59|83.3%|6|6|100%|
|SGuard.DataAnnotations.Is|5|5|10|76|50%|0|0||
|SGuard.DataAnnotations.SGuardBetweenAttribute|28|2|30|114|93.3%|28|32|87.5%|
|SGuard.DataAnnotations.SGuardCollectionItemsMatchAttribute|29|4|33|124|87.8%|17|22|77.2%|
|SGuard.DataAnnotations.SGuardCompareAttribute|4|0|4|25|100%|4|4|100%|
|SGuard.DataAnnotations.SGuardCreditCardAttribute|7|0|7|38|100%|2|2|100%|
|SGuard.DataAnnotations.SGuardEmailAddressAttribute|7|0|7|40|100%|2|2|100%|
|SGuard.DataAnnotations.SGuardGreaterThanAttribute|2|0|2|19|100%|0|0||
|SGuard.DataAnnotations.SGuardLessThanAttribute|2|0|2|17|100%|0|0||
|SGuard.DataAnnotations.SGuardMaxCountAttribute|18|2|20|87|90%|16|18|88.8%|
|SGuard.DataAnnotations.SGuardMaxLengthAttribute|4|0|4|25|100%|4|4|100%|
|SGuard.DataAnnotations.SGuardMinCountAttribute|18|2|20|87|90%|18|20|90%|
|SGuard.DataAnnotations.SGuardMinLengthAttribute|4|0|4|25|100%|4|4|100%|
|SGuard.DataAnnotations.SGuardPhoneAttribute|7|0|7|38|100%|2|2|100%|
|SGuard.DataAnnotations.SGuardRangeAttribute|8|0|8|42|100%|6|8|75%|
|SGuard.DataAnnotations.SGuardRegularExpressionAttribute|0|4|4|26|0%|0|4|0%|
|SGuard.DataAnnotations.SGuardRequiredCollectionAttribute|13|3|16|52|81.2%|11|14|78.5%|
|SGuard.DataAnnotations.SGuardRequiredIfAttribute|17|0|17|82|100%|13|14|92.8%|
|SGuard.DataAnnotations.SGuardStringLengthAttribute|4|0|4|24|100%|4|4|100%|
|SGuard.DataAnnotations.SGuardUrlAttribute|7|0|7|38|100%|2|2|100%|
|SGuard.DataAnnotations.SGuardValidationAttributeBase|19|1|20|83|95%|19|24|79.1%|
|SGuard.DataAnnotations.ThrowIf|14|2|16|101|87.5%|0|0||
<!-- TEST-RESULTS:END -->
