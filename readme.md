# <img src="/src/icon.png" height="30px"> Verify.Wolverine

[![Build status](https://ci.appveyor.com/api/projects/status/ddi2145ncdpw0wx0?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Wolverine)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Wolverine.svg)](https://www.nuget.org/packages/Verify.Wolverine/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for verifying [Wolverine](https://github.com/JasperFx/wolverine) types.


## NuGet package

https://nuget.org/packages/Verify.Wolverine/


## Usage

<!-- snippet: Enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Init()
{
    VerifyWolverine.Enable();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Handler

Given the handler:

<!-- snippet: Handler -->
<a id='snippet-handler'></a>
```cs
public record Message;

public record Response(string Property);

public class Handler
{
    IMessageContext context;

    public Handler(IMessageContext context) =>
        this.context = context;

    public ValueTask Handle(Message message) =>
        context.SendAsync(new Response("Property Value"));
}
```
<sup><a href='/src/Tests/Tests.cs#L31-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-handler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Test

Pass in instance of `RecordingMessageContext` in to the `Handle` method and then `Verify` that instance.

<!-- snippet: HandlerTest -->
<a id='snippet-handlertest'></a>
```cs
[Fact]
public async Task HandlerTest()
{
    var context = new RecordingMessageContext();
    var handler = new Handler(context);
    await handler.Handle(new Message());
    await Verify(context);
}
```
<sup><a href='/src/Tests/Tests.cs#L8-L19' title='Snippet source file'>snippet source</a> | <a href='#snippet-handlertest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will result in:

<!-- snippet: Tests.HandlerTest.verified.txt -->
<a id='snippet-Tests.HandlerTest.verified.txt'></a>
```txt
{
  CorrelationId: Guid_1,
  Sent: [
    {
      Message: {
        Property: Property Value
      }
    }
  ]
}
```
<sup><a href='/src/Tests/Tests.HandlerTest.verified.txt#L1-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.HandlerTest.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Wolverine](https://thenounproject.com/term/wolverine/3386573/) designed by [Phạm Thanh Lộc](https://thenounproject.com/thanhloc1009/) from [The Noun Project](https://thenounproject.com/).
