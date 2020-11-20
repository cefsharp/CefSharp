param(
    [ValidateSet("netcore31", "nupkg-only")]
    [Parameter(Position = 0)] 
    [string] $Target = "netcore31",
    [Parameter(Position = 1)]
    [string] $Version = "86.0.240",
    [Parameter(Position = 2)]
    [string] $AssemblyVersion = "86.0.240"
)

$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition
$CefSln = Join-Path $WorkingDir 'CefSharp3.netcore.sln'
$nuget = Join-Path $WorkingDir .\nuget\NuGet.exe

# Extract the current CEF Redist version from the CefSharp.Core\packages.CefSharp.Core.config file
# Save having to update this file manually Example 3.2704.1418
$CefSharpCorePackagesXml = [xml](Get-Content (Join-Path $WorkingDir 'CefSharp.Core\packages.CefSharp.Core.netcore.config'))
$RedistVersion = $CefSharpCorePackagesXml.SelectSingleNode("//packages/package[@id='cef.sdk']/@version").value

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

if (Test-Path Env:\APPVEYOR_BUILD_VERSION)
{
    $Version = $env:APPVEYOR_BUILD_VERSION
}

if ($env:APPVEYOR_REPO_TAG -eq "True")
{
    $Version = "$env:APPVEYOR_REPO_TAG_NAME".Substring(1)  # trim leading "v"
    $AssemblyVersion = $Version
    #Stip the -pre
    if($AssemblyVersion.Contains("-pre"))
    {
        $AssemblyVersion = $AssemblyVersion.Substring(0, $AssemblyVersion.IndexOf("-pre"))
    }
    Write-Diagnostic "Setting Version based on tag to $Version"    
    Write-Diagnostic "Setting AssemblyVersion based on tag to $AssemblyVersion"    
}

function Die 
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Message
    )

    Write-Host
    Write-Error $Message 
    exit 1
}

# https://github.com/jbake/Powershell_scripts/blob/master/Invoke-BatchFile.ps1
function Invoke-BatchFile 
{
   param(
        [Parameter(Position = 0, Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Path, 
        [Parameter(Position = 1, Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Parameters
   )

   $tempFile = [IO.Path]::GetTempFileName()  

   cmd.exe /c " `"$Path`" $Parameters && set > `"$tempFile`" " 

   Get-Content $tempFile | Foreach-Object {   
       if ($_ -match "^(.*?)=(.*)$")  
       { 
           Set-Content "env:\$($matches[1])" $matches[2]  
       } 
   }  

   Remove-Item $tempFile
}

function Msvs 
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [ValidateSet('Debug', 'Release')]
        [string] $Configuration, 

        [Parameter(Position = 1, ValueFromPipeline = $true)]
        [ValidateSet('x86', 'x64')]
        [string] $Platform
    )

    Write-Diagnostic "Targeting configuration $Configuration on platform $Platform"

    $Arguments = @(
        "$CefSln",
        "/t:build",
        "/p:Configuration=$Configuration",
        "/p:Platform=$Platform",
        "/verbosity:m"
    )
    
    &msbuild $Arguments
    
    if ($LastExitCode -ne 0)
    {
        Die "Build failed"
    }
}

function Compile
{
    Write-Diagnostic "Attempting to load vcvarsall.bat"
    
    $VS_VER=16;
    $VS_OFFICIAL_VER=2019;
    $programFilesDir = (${env:ProgramFiles(x86)}, ${env:ProgramFiles} -ne $null)[0]

    $vswherePath = Join-Path $programFilesDir 'Microsoft Visual Studio\Installer\vswhere.exe'
    #Check if we already have vswhere which is included in newer versions of VS2017/VS2019
    if(-not (Test-Path $vswherePath))
    {
        Write-Diagnostic "Downloading VSWhere as no install found at $vswherePath"
        
        # Check if we already have a local copy and download if required
        $vswherePath = Join-Path $WorkingDir \vswhere.exe
        
        # TODO: Check hash and download if hash differs
        if(-not (Test-Path $vswherePath))
        {
            $client = New-Object System.Net.WebClient;
            $client.DownloadFile('https://github.com/Microsoft/vswhere/releases/download/2.2.11/vswhere.exe', $vswherePath);
        }
    }
    
    Write-Diagnostic "VSWhere path $vswherePath"
    
    $versionSearchStr = "[$VS_VER.0," + ($VS_VER+1) + ".0)"
    $VS2019InstallPath = & $vswherePath -version $versionSearchStr -property installationPath
    
    if(-not (Test-Path $VS2019InstallPath))
    {
        Die "Visual Studio $VS_OFFICIAL_VER is not installed on your development machine, unable to continue."
    }
        
    $VXXCommonTools = Join-Path $VS2019InstallPath VC\Auxiliary\Build

    if ($VXXCommonTools -eq $null -or (-not (Test-Path($VXXCommonTools)))) {
        Die 'Error unable to find any visual studio environment'
    }

    $VCVarsAll = Join-Path $VXXCommonTools vcvarsall.bat
    if (-not (Test-Path $VCVarsAll)) {
        Die "Unable to find $VCVarsAll"
    }

    # Only configure build environment once
    if($env:CEFSHARP_BUILD_IS_BOOTSTRAPPED -eq $null)
    {
        Invoke-BatchFile $VCVarsAll 'x86'
        $env:CEFSHARP_BUILD_IS_BOOTSTRAPPED = $true
    }
    
    Write-Diagnostic "Restore Nuget Packages"

    # Restore packages
    . $nuget restore CefSharp.Core\packages.CefSharp.Core.netcore.config -PackagesDirectory packages
    . $nuget restore CefSharp.BrowserSubprocess.Core\packages.CefSharp.BrowserSubprocess.Core.netcore.config -PackagesDirectory packages
    &msbuild /t:restore CefSharp3.netcore.sln
    
    Write-Diagnostic "Compile Packages"
    
    # Compile
    Msvs 'Release' 'x64'
    Msvs 'Release' 'x86'
}

function Nupkg
{
    if (Test-Path Env:\APPVEYOR_PULL_REQUEST_NUMBER)
    {
        Write-Diagnostic "Pr Number: $env:APPVEYOR_PULL_REQUEST_NUMBER"
        Write-Diagnostic "Skipping Nupkg"
        return
    }

    Write-Diagnostic "Building nuget package"

    # Build newer style packages
    . $nuget pack nuget\PackageReference\CefSharp.Common.NETCore.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "RedistVersion=$RedistVersion;"
    . $nuget pack nuget\PackageReference\CefSharp.OffScreen.NETCore.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference
    . $nuget pack nuget\PackageReference\CefSharp.Wpf.NETCore.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference
    . $nuget pack nuget\PackageReference\CefSharp.WinForms.NETCore.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference
}

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

function WriteAssemblyVersion
{
    param()

    $Filename = Join-Path $WorkingDir CefSharp\Properties\AssemblyInfo.cs
    $Regex = 'public const string AssemblyVersion = "(.*)"';
    $Regex2 = 'public const string AssemblyFileVersion = "(.*)"'
    $Regex3 = 'public const string AssemblyCopyright = "Copyright © .* The CefSharp Authors"'
    
    $AssemblyInfo = Get-Content -Encoding UTF8 $Filename
    $CurrentYear = Get-Date -Format yyyy
    
    $NewString = $AssemblyInfo -replace $Regex, "public const string AssemblyVersion = ""$AssemblyVersion"""
    $NewString = $NewString -replace $Regex2, "public const string AssemblyFileVersion = ""$AssemblyVersion.0"""
    $NewString = $NewString -replace $Regex3, "public const string AssemblyCopyright = ""Copyright © $CurrentYear The CefSharp Authors"""
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

function WriteVersionToManifest($manifest)
{
    $Filename = Join-Path $WorkingDir $manifest
    $Regex = 'assemblyIdentity version="(.*?)"';
    
    $ManifestData = Get-Content -Encoding UTF8 $Filename
    $NewString = $ManifestData -replace $Regex, "assemblyIdentity version=""$AssemblyVersion.0"""
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

function WriteVersionToResourceFile($resourceFile)
{
    $Filename = Join-Path $WorkingDir $resourceFile
    $Regex1 = 'VERSION .*';
    $Regex2 = 'Version", ".*?"';
    $Regex3 = 'Copyright © .* The CefSharp Authors'
    
    $ResourceData = Get-Content -Encoding UTF8 $Filename
    $CurrentYear = Get-Date -Format yyyy
    #Assembly version with comma instead of dot
    $CppAssemblyVersion = $AssemblyVersion -replace '\.', ','
    
    $NewString = $ResourceData -replace $Regex1, "VERSION $CppAssemblyVersion"
    $NewString = $NewString -replace $Regex2, "Version"", ""$AssemblyVersion"""
    $NewString = $NewString -replace $Regex3, "Copyright © $CurrentYear The CefSharp Authors"
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

function WriteVersionToShfbproj
{
    $Filename = Join-Path $WorkingDir CefSharp.shfbproj
    $Regex1 = '<HelpFileVersion>.*<\/HelpFileVersion>';
    $Regex2 = '<HeaderText>Version .*<\/HeaderText>';
    
    $ShfbprojData = Get-Content -Encoding UTF8 $Filename
    $NewString = $ShfbprojData -replace $Regex1, "<HelpFileVersion>$AssemblyVersion</HelpFileVersion>"
    $NewString = $NewString -replace $Regex2, "<HeaderText>Version $AssemblyVersion</HeaderText>"
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

function WriteVersionToAppveyor
{
    $Filename = Join-Path $WorkingDir appveyor.yml
    $Regex1 = 'version: .*-CI{build}';
    
    $AppveyorData = Get-Content -Encoding UTF8 $Filename
    $NewString = $AppveyorData -replace $Regex1, "version: $AssemblyVersion-CI{build}"
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

function WriteVersionToNugetTargets
{
	$Filename = Join-Path $WorkingDir NuGet\PackageReference\CefSharp.Common.NETCore.targets
	
	Write-Diagnostic  "Write Version ($RedistVersion) to $Filename"
	$Regex1  = '" Version=".*"';
	$Replace = '" Version="' + $RedistVersion + '"';
	
	$RunTimeJsonData = Get-Content -Encoding UTF8 $Filename
	$NewString = $RunTimeJsonData -replace $Regex1, $Replace
	
	$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
	[System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

Write-Diagnostic "CEF Redist Version = $RedistVersion"

DownloadNuget

WriteAssemblyVersion
WriteVersionToShfbproj
WriteVersionToAppveyor
WriteVersionToNugetTargets

WriteVersionToManifest "CefSharp.BrowserSubprocess\app.manifest"
WriteVersionToManifest "CefSharp.OffScreen.Example\app.manifest"
WriteVersionToManifest "CefSharp.WinForms.Example\app.manifest"
WriteVersionToManifest "CefSharp.Wpf.Example\app.manifest"

WriteVersionToResourceFile "CefSharp.BrowserSubprocess.Core\Resource.rc"
WriteVersionToResourceFile "CefSharp.Core\Resource.rc"

switch -Exact ($Target)
{
    "nupkg-only"
    {
        Nupkg
    }
    "netcore31"
    {
        Compile
        Nupkg
    }
}
