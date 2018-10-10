# README (.NET Framework 4.0 support)

This file explains the .NET Framework 4.0 support that has been added back into CefSharp.

**Note: The support for .NET Framework 4.0 is unofficial and may be withdrawn at any time.**

# How is support for .NET Framework 4.0 accomplished?

CefSharp now provides assemblies that target .NET Framework 4.0, along with app.config files for these assemblies, that redirect to Microsoft's BCL assemblies (from NuGet packages), to provide functionality that although is supported in .NET CLR 4, was added to the Framework in a later version.

# Why am I being asked to reference Microsoft.Bcl.* / "Microsoft.Bcl.Build" when building .NET Framework 4.0 projects that contain a reference to CefSharp?

As the CefSharp assemblies, that are .NET Framework 4.0 compatible, reference the assemblies from these NuGet packages, the Microsoft.Bcl.* packages are installed along with it, and add targets to your project files.

Although the Microsoft.Bcl.* files are required by the project that references CefSharp code, they are not required by projects that only reference non-CefSharp code in that assembly/project.

Resolve this issue by adding the following to the first PropertyGroup element in your .csproj file that references CefSharp.

e.g.:
```xml
...
<PropertyGroup>
  <SkipValidatePackageReferences>true</SkipValidatePackageReferences>
</PropertyGroup>
...
```

This prevents the target from ensuring all projects that reference it, in turn also reference the Microsoft.Bcl.* assemblies.