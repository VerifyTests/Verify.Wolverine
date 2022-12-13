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

