
[![Build Status](https://travis-ci.org/thedmi/SentryDotNet.svg?branch=master)](https://travis-ci.org/thedmi/SentryDotNet)

# SentryDotNet

[![NuGet](https://img.shields.io/nuget/v/SentryDotNet.svg)](https://www.nuget.org/packages/SentryDotNet/)

SentryDotNet is a **.NET Standard 2.0 library** that implements just the generic parts of a [Sentry](https://sentry.io) client:

- Sentry API data model
- Basic mechanisms to construct Sentry events for various use cases
- Communication with the API

It's designed to be useful in any environment, be it web, desktop or mobile applications. The only external dependency is JSON.net.


## SentryDotNet.AspNetCore - ASP.NET Core Adapter for SentryDotNet

[![NuGet](https://img.shields.io/nuget/v/SentryDotNet.AspNetCore.svg)](https://www.nuget.org/packages/SentryDotNet.AspNetCore/)

This is a separate library that builds upon SentryDotNet and provides an **ASP.NET Core 2.0 middleware**, so that SentryDotNet can easily be used in web applications.


# Usage

## ASP.NET Core middleware

### Startup

After installing the [Nuget package](https://www.nuget.org/packages/SentryDotNet.AspNetCore), the middleware can be used from the Startup class:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSentryDotNet(new SentryClient("YourSentryDsn"));

    // Other services
}


public void Configure(IApplicationBuilder app)
{
    app.UseSentryDotNet();

    // Other middleware, e.g. app.UseMvc()
}
```

Make sure you `UseSentryDotNet()` *after* any middleware that intercepts exceptions. Otherwise, the SentryDotNet middleware will not see the exception. E.g. the `app.UseDeveloperExceptionPage()` should be used before `app.UseSentryDotNet()`.

Note that the `SentryClient` is thread-safe and can be shared across all requests (like in the code snippet above).

See also [the Startup class in the test project](SentryDotNet.AspNetCoreTestApp/Startup.cs) for an additional example.

### Additional Request Information

In `app.UseSentryDotNet()`, a new [SentryEventBuilder](SentryDotNet/SentryEventBuilder.cs) is created per request. This builder can be used to gather request data that is later sent to Sentry when an exception occurs. The builder can be accessed through the HTTP context items  with `HttpContext.Items[SentryDotNetMiddleware.EventBuilderKey]`.

The following example in an authorization handler illustrates this. Note that the mechanism works anywhere the HTTP context can be accessed.

```csharp
// Somewhere in the authorization handler

var sentryEventBuilder = (SentryEventBuilder)httpContext.Items[SentryDotNetMiddleware.EventBuilderKey];
sentryEventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("authorized") { Message = $"User {username} authorized"});

```

In this example, information about the logged in user is sent along with errors (should they occur).


## Generic SentryClient

If you're on something else than ASP.NET Core, you can use the [SentryClient](SentryDotNet/SentryClient.cs) directly. Just install [this Nuget package](https://www.nuget.org/packages/SentryDotNet/).

For simple scenarios, create a `SentryClient` and use it to capture exceptions and messages. The `SentryClient` is thread-safe and can be shared across all threads.

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

## Advanced Scenarios

The following customizations can be used with the generic library or the ASP.NET Core adapter.

### Custom HTTP request processing

In some cases, applications need to be able to send the Sentry requests themselves. This could be necessary e.g. when resiliency policies must be used (e.g. through the excellent [Polly library](https://github.com/App-vNext/Polly)). The following example illustrates this:

```csharp
var httpClient = new HttpClient();
var retryPolicy = Polly.Handle<SentryClientException>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

var sentryClient =  new SentryClient(
    "YourSentryDsn",
    async (r) => await retryPolicy.ExecuteAsync(async () => await httpClient.SendAsync(r)));

// Now use sentryClient the same way as above
```

### Sentry event building

Since the `SentryClient` itself is stateless & thread-safe, it cannot be used directly to accumulate additional sentry event information such as breadcrumbs. Instead, a `SentryEventBuilder` can be created to do this. An example where this would be useful is in web applications. You could create one `SentryEventBuilder` per request and add request-specific data to the builder. When an exception occurs, you can use the builder to capture the exception together with the previously recorded data.


```csharp
// Somewhere in the request bootstrapping code...

// Create an event builder and add custom data
var eventBuilder = sentryClient.CreateEventBuilder();
eventBuilder.Culprit = RequestUri.ToString();
eventBuilder.Breadcrumbs.Add(new SentryBreadcrumb("Referer") { Message = Request.Headers.Referer });

// Attach the builder to the request context for later retrieval
RequestContext["SentryEventBuilder"] = eventBuilder;

// Later in the exception handling pipeline...

var eventBuilder = RequestContext["SentryEventBuilder"] as SentryEventBuilder;
eventBuilder.SetException(theException);

// Send to Sentry
await eventBuilder.CaptureAsync();
```

Note that the above is just an example, you will need to adapt the code to the
specific web framework you're using.


# Other Sentry clients

If you don't like SentryDotNet, here are a few other libraries. SentryDotNet borrowed ideas & code from a few of them (thanks!).

- [Raven C#](https://github.com/getsentry/raven-csharp)
- [Sentinel](https://github.com/PrestigeXP/Sentinel)
