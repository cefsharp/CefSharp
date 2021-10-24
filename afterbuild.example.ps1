param(
	[Parameter(Position = 0, Mandatory = $true)] 
	[string] $Version
)

$WorkingDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Nuget = Join-Path $env:LOCALAPPDATA .\nuget\NuGet.exe
$PackagePath = Join-Path $WorkingDir\nuget

function Write-Diagnostic 
{
    param(
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [string] $Message
    )

    Write-Host
    Write-Host $Message -ForegroundColor Green
    Write-Host
}

Write-Diagnostic "Uploading packages to myget"

$Packages = @(
	"$PackagePath\CefSharp.Common.$Version.nupkg",
	"$PackagePath\CefSharp.WinForms.$Version.nupkg",
	"$PackagePath\CefSharp.Wpf.$Version.nupkg"
) 

$Packages | ForEach-Object {
	. $Nuget push $_ -Source https://myget.org/F/X
}