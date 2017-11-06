
[![Build Status](https://travis-ci.org/thedmi/SentryDotNet.svg?branch=master)](https://travis-ci.org/thedmi/SentryDotNet)
[![NuGet](https://img.shields.io/nuget/v/SentryDotNet.svg)](https://www.nuget.org/packages/SentryDotNet/)

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


## Usage

### Simple

For simple scenarios, just create a `SentryClient` and use it to capture exceptions and messages. The `SentryClient`
is thread-safe and can be shared across all threads.

```csharp
// During app initialization, instantiate the client
var client = new SentryClient("YourSentryDsn");

// Somewhere else...
try {
    DoSomething();
}
catch (Exception e) {
    // Send exceptions to Sentry
    await client.CaptureAsync(exception);
    throw;
}
```


### Custom HTTP request processing

In some cases, applications need to be able to send the Sentry requests themselves. This could be necessary when a
shared `HttpClient` singleton should be used (see e.g. [this blog post](https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/))
or when resiliency policies must be used (e.g. through the excellent [Polly library](https://github.com/App-vNext/Polly)). The
following example shows both:

```csharp
var singletonHttpClient = new HttpClient();
var retryPolicy = Polly.Handle<SentryClientException>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

var sentryClient =  new SentryClient(
    "YourSentryDsn",
    async (r) => await retryPolicy.ExecuteAsync(async () => await singletonHttpClient.SendAsync(r)));

// Now use sentryClient the same way as above
```

### Sentry event building

Since the `SentryClient` itself is stateless & thread-safe, it cannot be used directly to
accumulate information. Instead, a `SentryEventBuilder` can be used to do this. An example where this would be useful is in web applications. You could create one `SentryEventBuilder` per request and add request-specific data to the builder. When an exception occurs, you can use the builder to capture the exception together with the previously recorded data.


```csharp
// Somewhere in the request bootstrapping code...

// Create an event builder and add custom data
var eventBuilder = sentryClient.CreateEventBuilder();
eventBuilder.Culprit = Request.Uri.ToString();
eventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("Referer") { Message = Request.Headers.Referer });

// Attach the builder to the request context for later retrieval
Request.Context["SentryEventBuilder"] = eventBuilder;

// Later in the exception handling pipeline...

var eventBuilder = Request.Context["SentryEventBuilder"] as SentryEventBuilder;
eventBuilder.SetException(theException);

// Send to Sentry
await eventBuilder.CaptureAsync();
```

Note that the above is just an example, you will need to adapt the code to the
specific web framework you're using.


## Other Sentry clients

If you don't like SentryDotNet, here are a few other, more opinionated libraries. SentryDotNet borrowed ideas & code from a few of them (thanks!).

- [Raven C#](https://github.com/getsentry/raven-csharp)
- [Sentinel](https://github.com/PrestigeXP/Sentinel)
