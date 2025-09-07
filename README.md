# SGuard.DataAnnotations

SGuard.DataAnnotations is an extension library for [SGuard](https://github.com/selcukgural/sguard), providing expressive guard clauses and validation for .NET objects using DataAnnotations.

## Pattern Inheritance from SGuard

All `Is.*` and `ThrowIf.*` methods in SGuard.DataAnnotations strictly follow the [SGuard pattern](https://github.com/selcukgural/sguard):

- **Is.* methods**: Return booleans for validation checks, never throw exceptions, and invoke callbacks with `GuardOutcome.Success` when the result is true and `GuardOutcome.Failure` when false.
- **ThrowIf.* methods**: Throw exceptions when validation fails, support custom exception types and constructor arguments, and invoke callbacks with `GuardOutcome.Failure` before throwing and `GuardOutcome.Success` when validation passes.
- **Callback Model**: Unified and consistent with SGuard. Callbacks are always invoked for both success and failure outcomes (unless arguments are invalid).
- **Custom Exception Support**: Overloads for custom exception types and constructor arguments are provided for all exception-throwing guards.

For more details on the SGuard pattern and usage, see the [SGuard main documentation](https://github.com/selcukgural/sguard).

## Usage Example

```csharp
// Boolean guard (never throws)
bool isValid = Is.DataAnnotationsValid(obj, callback: outcome => Console.WriteLine($"Outcome: {outcome}"));

// Exception-throwing guard
ThrowIf.DataAnnotationsInValid(obj, callback: outcome => Console.WriteLine($"Outcome: {outcome}"));

// Custom exception
ThrowIf.DataAnnotationsInValid<MyCustomException>(obj, new MyCustomException("Validation failed!"));
```

## License

This project is licensed under the MIT License.

