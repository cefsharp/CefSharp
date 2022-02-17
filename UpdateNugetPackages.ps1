#requires -Version 5

# Update projects files
# I haven't found a clean solution that allows for using just nuget.exe and dotnet.exe to do this
# Update the vcxproj files first
# Update the .Net csproj files modifying the xml file directly

$CefVersion = '98.1.21'
$CefSharpVersion = $CefVersion + "0"

function RemoveEnsureNuGetPackageBuildImports
{
    param([Parameter(Position = 0, ValueFromPipeline = $true)][string] $FileName)

    $xml = [xml](Get-Content $FileName)
	$target = $xml.Project.Target | Where-Object {$_."Name" -eq "EnsureNuGetPackageBuildImports"}
	
	if($target -ne $null)
	{
		$target.ParentNode.RemoveChild($target)

		$xml.Save( $FileName )
	}
}

$vcxprojFiles = @('CefSharp.Core.Runtime\CefSharp.Core.Runtime.vcxproj','CefSharp.BrowserSubprocess.Core\CefSharp.BrowserSubprocess.Core.vcxproj')

foreach($file in $vcxprojFiles)
{
	..\nuget update $file -Id cef.sdk -Version $CefVersion
	
	RemoveEnsureNuGetPackageBuildImports (Resolve-Path $file)
}

$vcxprojFiles = @('CefSharp.Core.Runtime\CefSharp.Core.Runtime.netcore.vcxproj', 'CefSharp.BrowserSubprocess.Core\CefSharp.BrowserSubprocess.Core.netcore.vcxproj')

foreach($file in $vcxprojFiles)
{
	..\nuget update $file -Id cef.sdk -Version $CefVersion
	
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
	
	$packRef = $xml.Project.ItemGroup.PackageReference | Where-Object {$_."Include" -eq "chromiumembeddedframework.runtime"}
	
	$packRef.Version = $RedistVersion
	
	$xml.Save( $file )
}

.\build.ps1 -Target update-build-version -Version $CefSharpVersion -AssemblyVersion $CefSharpVersion