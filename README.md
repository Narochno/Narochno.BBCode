# Narochno.BBCode [![Build status](https://ci.appveyor.com/api/projects/status/7p7owa4ksp8w2jk2/branch/master?svg=true)](https://ci.appveyor.com/project/Narochno/narochno-bbcode/branch/master) [![NuGet](https://img.shields.io/nuget/v/Narochno.BBCode.svg)](https://www.nuget.org/packages/Narochno.BBCode/)
A fork of https://bbcode.codeplex.com/ for .NET Core.

## Example Usage

![Example](example.png)

```csharp
var parser = new BBCodeParser();

var result = parser.ToHtml("[b]bold[/b]");

// result is <b>bold</b>
```
