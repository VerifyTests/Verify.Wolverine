# <img src="/src/icon.png" height="30px"> Verify.Wolverine

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/ddi2145ncdpw0wx0?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Wolverine)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Wolverine.svg)](https://www.nuget.org/packages/Verify.Wolverine/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for verifying [Wolverine](https://github.com/JasperFx/wolverine) via a custom test context.

Uses the same pattern as the [Wolverine TestMessageContext](https://wolverine.netlify.app/guide/testing.html#testmessagecontext) with some additions:

 * All messaging parameters, eg DeliveryOptions and timeout, can be asserted.
 * Support for `IMessageBus.InvokeAsync<T>` via [AddInvokeResult](#addinvokeresult).

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](docs/zzz.png)](https://entityframework-extensions.net)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.Wolverine


## Usage

<!-- snippet: Enable -->
<a id='snippet-Enable'></a>
```cs
[ModuleInitializer]
public static void Init() =>
    VerifyWolverine.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Handler

Given the handler:

<!-- snippet: Handler -->
<a id='snippet-Handler'></a>
```cs
public class Handler(IMessageBus context)
{
    public ValueTask Handle(Message message) =>
        context.SendAsync(new Response("Property Value"));
}
```
<sup><a href='/src/Tests/Tests.cs#L30-L38' title='Snippet source file'>snippet source</a> | <a href='#snippet-Handler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Test

Pass in instance of `RecordingMessageContext` in to the `Handle` method and then `Verify` that instance.

<!-- snippet: HandlerTest -->
<a id='snippet-HandlerTest'></a>
```cs
[Fact]
public async Task HandlerTest()
{
    var context = new RecordingMessageContext();
    var handler = new Handler(context);
    await handler.Handle(new Message("value"));
    await Verify(context);
}
```
<sup><a href='/src/Tests/Tests.cs#L7-L18' title='Snippet source file'>snippet source</a> | <a href='#snippet-HandlerTest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will result in:

<!-- snippet: Tests.HandlerTest.verified.txt -->
<a id='snippet-Tests.HandlerTest.verified.txt'></a>
```txt
{
  Sent: [
    {
      Message: {
        Property: Property Value
      }
    }
  ]
}
```
<sup><a href='/src/Tests/Tests.HandlerTest.verified.txt#L1-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.HandlerTest.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### AddInvokeResult

When using [Request/Reply](https://wolverine.netlify.app/guide/messaging/message-bus.html#request-reply) via `IMessageBus.InvokeAsync<T>` the message context is required to supply the "Reply" part. This can be one using `RecordingMessageContext.AddInvokeResult<T>`.

For example, given the handler:

<!-- snippet: InvokeAsyncHandler -->
<a id='snippet-InvokeAsyncHandler'></a>
```cs
public class Handler(IMessageBus context)
{
    public async Task Handle(Message message)
    {
        var request = new Request(message.Property);
        var response = await context.InvokeAsync<Response>(request);
        Trace.WriteLine(response.Property);
    }
}
```
<sup><a href='/src/Tests/InvokeDelegateUsage.cs#L26-L38' title='Snippet source file'>snippet source</a> | <a href='#snippet-InvokeAsyncHandler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The result can be set:

<!-- snippet: InvokeDelegateTest -->
<a id='snippet-InvokeDelegateTest'></a>
```cs
[Fact]
public async Task HandlerTest()
{
    var context = new RecordingMessageContext();
    context.AddInvokeResult<Response>(
        message =>
        {
            var request = (Request) message;
            return new Response(request.Property);
        });
    var handler = new Handler(context);
    await handler.Handle(new Message("value"));
    await Verify(context);
}
```
<sup><a href='/src/Tests/InvokeDelegateUsage.cs#L7-L24' title='Snippet source file'>snippet source</a> | <a href='#snippet-InvokeDelegateTest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Wolverine](https://thenounproject.com/term/wolverine/3386573/) designed by [Phạm Thanh Lộc](https://thenounproject.com/thanhloc1009/) from [The Noun Project](https://thenounproject.com/).
