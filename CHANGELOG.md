# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
<!-- Released as 0.2.0: rename this heading to "## [0.2.0] - YYYY-MM-DD" in the release commit. -->
### Changed
- **Breaking:** targets net8.0, net9.0 and net10.0. .NET 6 and 7 are out of support and no longer targeted; stay on
  0.1.1 for them.
- Depends on SGuard 3.0.0 instead of SGuard 0.1.1. Before, apps without their own SGuard reference got SGuard 0.1.1,
  which has known issues that are fixed in later releases.
- The dependency on System.ComponentModel.Annotations is removed: it's part of .NET.
- **Breaking:** `SGuardCompare`, `SGuardGreaterThan`, `SGuardLessThan`, `SGuardBetween` and `SGuardRequiredIf` throw
  `InvalidOperationException` when they name a property the model doesn't have, instead of returning an
  "Unknown property" validation error. Validation messages never contain property or type names: values that can't be
  compared (different types, or not `IComparable`) fail with the attribute's own localized message.
- **Breaking:** `SGuardRequiredCollection`, `SGuardMinCount`, `SGuardMaxCount` and `SGuardCollectionItemsMatch` throw
  `InvalidOperationException` for an `IQueryable` instead of running the query.
- **Breaking:** `SGuardCollectionItemsMatch` throws `ArgumentException` for a validator type that isn't a creatable
  `ValidationAttribute`. Before, such a type was skipped and every item passed.

### Added
- `ThrowIf.DataAnnotationsInValid(instance, () => new MyException(...))`: takes a factory that runs only when
  validation fails.
- `SGuardCollectionItemsMatch.MaxErrors` (default 10): `AggregateAllErrors` stops after this many errors.
- Symbol package (.snupkg) and package validation against the previous release.

### Deprecated
- `ThrowIf.DataAnnotationsInValid<TException>(instance, object[] constructorArgs)` (warning `SGUARDDA001`): it creates
  the exception with reflection, which the compiler can't check. Use the factory overload instead.

## [0.1.1] - 2025-09-14
### Added
- The package README on nuget.org.

## [0.1.0] - 2025-09-09
### Added
- Initial release of SGuard.DataAnnotations library.
- Core validation attributes and guards.
- Multilingual resource support.
- Unit tests and CI/CD workflow.

