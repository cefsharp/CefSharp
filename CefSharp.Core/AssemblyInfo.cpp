#include "Stdafx.h"

using namespace System;
using namespace System::Reflection;
using namespace System::Runtime::CompilerServices;
using namespace System::Runtime::InteropServices;
using namespace System::Security::Permissions;
using namespace CefSharp;

[assembly:AssemblyTitle("CefSharp.Core")];
[assembly:AssemblyCompany(AssemblyInfo::AssemblyCompany)];
[assembly:AssemblyProduct(AssemblyInfo::AssemblyProduct)];
[assembly:AssemblyCopyright(AssemblyInfo::AssemblyCopyright)]

[assembly:AssemblyVersion(AssemblyInfo::AssemblyVersion)];
[assembly:ComVisible(AssemblyInfo::ComVisible)];
[assembly:CLSCompliant(AssemblyInfo::ClsCompliant)];
[assembly:SecurityPermission(SecurityAction::RequestMinimum, UnmanagedCode = true)];

[assembly:AssemblyDescription("")]
[assembly:AssemblyConfiguration("")]
[assembly:AssemblyTrademark("")]
[assembly:AssemblyCulture("")]

[assembly:InternalsVisibleTo(AssemblyInfo::CefSharpBrowserSubprocessProject)];
[assembly:InternalsVisibleTo(AssemblyInfo::CefSharpWpfProject)];
[assembly:InternalsVisibleTo(AssemblyInfo::CefSharpWinFormsProject)];
[assembly:InternalsVisibleTo(AssemblyInfo::CefSharpTestProject)];
