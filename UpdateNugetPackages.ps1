#requires -Version 5
[CmdletBinding()]

param(
	[Parameter(Position = 1)]
	[string] $CefVersion = "144.0.15",
	[Parameter(Position = 2)]
	[string] $CefSharpVersion = ""
	)

# Update projects files
# I haven't found a clean solution that allows for using just nuget.exe and dotnet.exe to do this
# Update the vcxproj files first
# Update the .Net csproj files modifying the xml file directly
Set-StrictMode -version latest;
$ErrorActionPreference = "Stop";

function DownloadNuget()
{
	if(-not (Test-Path $nuget))
	{
		$client = New-Object System.Net.WebClient;
		$client.DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe', $nuget);
	}

	if(-not (Test-Path $nuget))
	{
		Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
	}
}

function RemoveEnsureNuGetPackageBuildImports
{
	param([Parameter(Position = 0, ValueFromPipeline = $true)][string] $FileName)

	$xml = [xml](Get-Content $FileName)
	$ns = new-object Xml.XmlNamespaceManager $xml.NameTable
	$ns.AddNamespace("ns", $xml.DocumentElement.NamespaceURI)

	$target = $xml.SelectSingleNode("//ns:Project/ns:Target[@Name='EnsureNuGetPackageBuildImports']", $ns);
	
	if($null -ne $target)
	{
		$target.ParentNode.RemoveChild($target)

		$xml.Save( $FileName )
	}
}

function WriteVersionToPowershellBuildScript
{
	param([Parameter(Position = 0, ValueFromPipeline = $true)][string] $VersionNo)
	
    $Filename = Join-Path $WorkingDir build.ps1
    
    $buildScriptData = Get-Content -Encoding UTF8 $Filename
    $NewString = $buildScriptData -replace 'Version = "[\d\.]+"', ('Version = "' + $VersionNo + '"')
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

if ($CefSharpVersion -eq "")
{
	$CefSharpVersion = $CefVersion + "0"
}

$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition
$nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
DownloadNuget

$vcxprojFiles = @('CefSharp.Core.Runtime\CefSharp.Core.Runtime.vcxproj','CefSharp.BrowserSubprocess.Core\CefSharp.BrowserSubprocess.Core.vcxproj')

foreach($file in $vcxprojFiles)
{
	. $nuget update $file -Id cef.sdk -Version $CefVersion
	
	RemoveEnsureNuGetPackageBuildImports (Resolve-Path $file)
}

$vcxprojFiles = @('CefSharp.Core.Runtime\CefSharp.Core.Runtime.netcore.vcxproj', 'CefSharp.BrowserSubprocess.Core\CefSharp.BrowserSubprocess.Core.netcore.vcxproj')

foreach($file in $vcxprojFiles)
{
	. $nuget update $file -Id cef.sdk -Version $CefVersion
	
	RemoveEnsureNuGetPackageBuildImports (Resolve-Path $file)
}

#Read the newly updated version number from the packages.CefSharp.Core.Runtime.config

$CefSharpCorePackagesXml = [xml](Get-Content (Resolve-Path 'CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.config'))
$RedistVersion = $CefSharpCorePackagesXml.SelectSingleNode("//packages/package[@id='cef.sdk']/@version").value

$csprojFiles = @('CefSharp.WinForms.Example\CefSharp.WinForms.Example.netcore.csproj','CefSharp.Wpf.Example\CefSharp.Wpf.Example.netcore.csproj','CefSharp.OffScreen.Example\CefSharp.OffScreen.Example.netcore.csproj', 'CefSharp.Test\CefSharp.Test.netcore.csproj', 'CefSharp.WinForms.Example\CefSharp.WinForms.Example.csproj','CefSharp.Wpf.Example\CefSharp.Wpf.Example.csproj','CefSharp.OffScreen.Example\CefSharp.OffScreen.Example.csproj', 'CefSharp.Test\CefSharp.Test.csproj', 'CefSharp.Wpf.HwndHost.Example\CefSharp.Wpf.HwndHost.Example.netcore.csproj', 'CefSharp.Wpf.HwndHost.Example\CefSharp.Wpf.HwndHost.Example.csproj')

#Loop through the net core example projects and update the package version number

foreach($file in $csprojFiles)
{
	$file = Resolve-Path $file
	$xml = New-Object xml
	$xml.PreserveWhitespace = $true
	$xml.Load($file)

	$packRef = $xml.SelectSingleNode("//Project/ItemGroup/PackageReference[@Include='chromiumembeddedframework.runtime']");
	$packRef.Version = $RedistVersion	
	
	$xml.Save( $file )
}

WriteVersionToPowershellBuildScript $CefSharpVersion

.\build.ps1 -Target update-build-version -Version $CefSharpVersion -AssemblyVersion $CefSharpVersion