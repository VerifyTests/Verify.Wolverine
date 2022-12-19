# <img src="/src/icon.png" height="30px"> Verify.Wolverine

[![Build status](https://ci.appveyor.com/api/projects/status/07apa0wm0lxulr5o?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-Wolverine)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Wolverine.svg)](https://www.nuget.org/packages/Verify.Wolverine/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for verifying [Moq](https://github.com/moq/moq4) types.


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

    public void Handle(Message message) =>
        context.SendAsync(new Response("Property Value"));
}
```
<sup><a href='/src/Tests/Tests.cs#L22-L38' title='Snippet source file'>snippet source</a> | <a href='#snippet-handler' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Test

<!-- snippet: HandlerTest -->
<a id='snippet-handlertest'></a>
```cs
[Fact]
public Task HandlerTest()
{
    var context = new RecordingMessageContext();
    var handler = new Handler(context);
    handler.Handle(new Message());
    return Verify(context);
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
