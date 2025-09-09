# Commit Message Guidelines

To maintain a clean, understandable, and professional project history, please follow these guidelines when writing commit messages for SGuard.DataAnnotations:

## General Principles
- Use English for all commit messages.
- Be concise but descriptive. Explain the "what" and "why" of the change.
- Use the imperative mood (e.g., "Add validation for URLs," "Fix bug in guard logic").
- Reference related issues or pull requests when relevant (e.g., "Fixes #42").

## Structure
A typical commit message should consist of:

```
<type>: <short summary>

[Optional body]
[Optional footer]
```

### Types
- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation only changes
- **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semicolons, etc.)
- **refactor**: A code change that neither fixes a bug nor adds a feature
- **perf**: A code change that improves performance
- **test**: Adding missing tests or correcting existing tests
- **chore**: Changes to the build process or auxiliary tools and libraries

### Example
```
feat: Add SGuardUrlAttribute for localized URL validation

- Implements custom attribute wrapping UrlAttribute
- Adds support for resource-based error messages
- Updates documentation
```

## Best Practices
- Limit the subject line to 50 characters.
- Capitalize the subject line.
- Do not end the subject line with a period.
- Use the body to explain "why" the change was made, if necessary.
- Separate the subject from the body with a blank line.
- Reference issues and PRs in the footer if applicable.

## References
- [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
- [How to Write a Git Commit Message](https://chris.beams.io/posts/git-commit/)

---
Following these guidelines helps everyone understand the project history and makes collaboration easier!

