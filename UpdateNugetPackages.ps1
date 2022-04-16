#requires -Version 5
[CmdletBinding()]

param(
	[Parameter(Position = 1)]
	[string] $CefVersion = "100.0.12",
	[Parameter(Position = 2)]
	[string] $CefSharpVersion = "",
	[Parameter(Position = 3)]
	[string] $BuildArches = "x86 x64 arm64"
	)
# Update projects files
# I haven't found a clean solution that allows for using just nuget.exe and dotnet.exe to do this
# Update the vcxproj files first
# Update the .Net csproj files modifying the xml file directly
$ARCHES = [System.Collections.ArrayList]$BuildArches.ToLower().Split(" ");
Set-StrictMode -version latest;
$ErrorActionPreference = "Stop";

if ($CefSharpVersion -eq "")
{
	$CefSharpVersion = $CefVersion + "0"
}

function UpdateNupsecCommonArches()
{
	$nuspecFile = (Resolve-Path 'NuGet\CefSharp.Common.nuspec');
	$CefSharpNugetXml = [xml](Get-Content $nuspecFile );
	$ns = new-object Xml.XmlNamespaceManager $CefSharpNugetXml.NameTable
	$ns.AddNamespace("ns", $CefSharpNugetXml.DocumentElement.NamespaceURI)

	$srcNode = $CefSharpNugetXml.SelectSingleNode("//ns:package/ns:metadata/ns:dependencies/ns:group/ns:dependency",$ns);

	$parentNode = $srcNode.parentNode;
	$parentNode.RemoveAll();


	$ARCHES.Remove("arm64")

	foreach ($arch in $ARCHES)
	{
		$clone = $srcNode.CloneNode($true);
		$clone.SetAttribute("id","cef.redist." + $arch);
		$parentNode.AppendChild( $clone ) | Out-Null;


	}

	if ( $parentNode.get_HasChildNodes() -eq $false )
	{
		$parentNode.AppendChild( $srcNode ); # we need to keep one node on there as it is used as a template, for building other arches this group is ignored anyway
	}

	$CefSharpNugetXml.Save( $nuspecFile )
}

UpdateNupsecCommonArches

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
$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition
$nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
DownloadNuget


function RemoveEnsureNuGetPackageBuildImports
{
	param([Parameter(Position = 0, ValueFromPipeline = $true)][string] $FileName)

	$xml = [xml](Get-Content $FileName)
	$target = $xml.SelectSingleNode("//Project/Target[@Name='EnsureNuGetPackageBuildImports']");
	
	if($target -ne $null)
	{
		$target.ParentNode.RemoveChild($target)

		$xml.Save( $FileName )
	}
}

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

$csprojFiles = @('CefSharp.WinForms.Example\CefSharp.WinForms.Example.netcore.csproj','CefSharp.Wpf.Example\CefSharp.Wpf.Example.netcore.csproj','CefSharp.OffScreen.Example\CefSharp.OffScreen.Example.netcore.csproj', 'CefSharp.Test\CefSharp.Test.netcore.csproj', 'CefSharp.WinForms.Example\CefSharp.WinForms.Example.csproj','CefSharp.Wpf.Example\CefSharp.Wpf.Example.csproj','CefSharp.OffScreen.Example\CefSharp.OffScreen.Example.csproj', 'CefSharp.Test\CefSharp.Test.csproj')

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

.\build.ps1 -Target update-build-version -Version $CefSharpVersion -AssemblyVersion $CefSharpVersion
