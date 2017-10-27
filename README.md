
# SentryDotNet - An unopinionated Sentry client for .NET

SentryDotNet is a **.NET Standard 2.0 library** that implements just the basic parts of a [Sentry](https://sentry.io) client:

- Sentry API data model
- Basic mechanisms to construct Sentry events for various use cases
- Communication with the API

This library does **not** attempt to provide out of the box integrations with various frameworks such as ASP.NET Core or Xamarin, but it is a good starting point if you want to build such an integration.

## Design Goals

- Target .NET Standard 2.0
- Only json.NET as external dependency
- Suitable for all environments (web apps, mobile apps, desktop apps)
- Simple to use


## Other Sentry clients

If you don't like SentryDotNet, here are a few other, more opinionated libraries. SentryDotNet borrowed ideas & code from a few of them (thanks!).

- [Raven C#](https://github.com/getsentry/raven-csharp)
- [Sentinel](https://github.com/PrestigeXP/Sentinel)
