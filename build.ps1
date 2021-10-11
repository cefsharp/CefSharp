param(
    [ValidateSet("vs2022","vs2019", "netcore31", "nupkg-only")]
    [Parameter(Position = 0)] 
    [string] $Target = "vs2019",
    [Parameter(Position = 1)]
    [string] $Version = "94.4.20",
    [Parameter(Position = 2)]
    [string] $AssemblyVersion = "94.4.20",
    [Parameter(Position = 3)]
    [ValidateSet("NetFramework", "NetCore", "NetFramework452", "NetCore31")]
    [string] $TargetFramework = "NetFramework",
    [Parameter(Position = 4)]
    [string] $BuildArches = "x86 x64 amd64"
)
Set-StrictMode -version latest;
$ErrorActionPreference = "Stop";
$IsNetCoreBuild = $TargetFramework.Contains("NetCore")

$ARCHES = $BuildArches.Split(" ");
$ARCHES_TO_BITKEY = @{};
foreach ($arch in $ARCHES) {
    $arch_bit = $arch;
    if ($arch_bit.StartsWith("x")) {
        $arch_bit = $arch.Substring(1);
        if ($arch_bit -eq "86"){
            $arch_bit = "32";
        }
        $ARCHES_TO_BITKEY[$arch] = $arch_bit;
    }
}

function TernaryReturn 
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [bool] $Yes,
        [Parameter(Position = 1, ValueFromPipeline = $true)]
        $Value,
        [Parameter(Position = 2, ValueFromPipeline = $true)]
        $Value2
    )

    if($Yes) {
        return $Value
    }
    
    $Value2
}


$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition
$CefSln = Join-Path $WorkingDir ('CefSharp3' + (TernaryReturn $IsNetCoreBuild ".netcore" "") + '.sln')

# Extract the current CEF Redist version from the CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.config file
# Save having to update this file manually Example 3.2704.1418
$CefSharpCorePackagesXml = [xml](Get-Content (Join-Path $WorkingDir ('CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime' + (TernaryReturn $IsNetCoreBuild ".netcore" "") + '.config')))
$RedistVersion = $CefSharpCorePackagesXml.SelectSingleNode("//packages/package[@id='cef.sdk']/@version").value
$nuget = Join-Path $WorkingDir .\nuget\NuGet.exe

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

function Warn 
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Message
    )

    Write-Host
    Write-Host $Message -ForegroundColor Yellow
    Write-Host
}


function Msvs 
{
    param(
        [ValidateSet('v142','v143','netcore')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain, 

        [Parameter(Position = 1, ValueFromPipeline = $true)]
        [ValidateSet('Debug', 'Release')]
        [string] $Configuration, 

        [Parameter(Position = 2, ValueFromPipeline = $true)]
        [ValidateSet('x86', 'x64', 'arm64')]
        [string] $Platform
    )

    Write-Diagnostic "Targeting $Toolchain using configuration $Configuration on platform $Platform"

    $VisualStudioVersion = $null
    $VXXCommonTools = $null
    $VS_VER = -1
    $VS_OFFICIAL_VER = -1
    $VS_PRE = $false;

    switch -Exact ($Toolchain)
	{
        'v142'
		{
            $VS_VER = 16;
            $VS_OFFICIAL_VER = 2019;
        }
        'v143'
        {
            $VS_VER = 17;
            $VS_OFFICIAL_VER = 2022;
            $VS_PRE = $true;
        }
    }
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

    $preStr = TernaryReturn $VS_PRE "-prerelease" ""
    $VSInstallPath = & $vswherePath -version $versionSearchStr -property installationPath $preStr
    
    Write-Diagnostic "$($VS_OFFICIAL_VER)InstallPath: $VSInstallPath"
        
    if( -not $VSInstallPath -or -not (Test-Path $VSInstallPath))
    {
        Die "Visual Studio $VS_OFFICIAL_VER is not installed on your development machine, unable to continue, ran command: $vswherePath -version $versionSearchStr -property installationPath"
    }
        
    $MSBuildExe = "msbuild.exe"
    $VisualStudioVersion = "$VS_VER.0"
    $VXXCommonTools = Join-Path $VSInstallPath VC\Auxiliary\Build


    if ($VXXCommonTools -eq $null -or (-not (Test-Path($VXXCommonTools)))) {
        Die 'Error unable to find any visual studio environment'
    }

    $VCVarsAll = Join-Path $VXXCommonTools vcvarsall.bat
    if (-not (Test-Path $VCVarsAll)) {
        Die "Unable to find $VCVarsAll"
    }

    # Only configure build environment once
    if($env:CEFSHARP_BUILD_IS_BOOTSTRAPPED -eq $null) {
        Invoke-BatchFile $VCVarsAll $Platform
        $env:CEFSHARP_BUILD_IS_BOOTSTRAPPED = $true
    }

    $Arch = $Platform
    if ($Arch -eq "x86"){
        $Arch="win32";
    }

	# Restore Nuget packages
	&msbuild /t:restore /p:Platform=$Arch /p:Configuration=Release $CefSln

    $Arguments = @(
        "$CefSln",
        "/t:rebuild",
        "/p:VisualStudioVersion=$VisualStudioVersion",
        "/p:Configuration=$Configuration",
        "/p:Platform=$Arch",
        "/verbosity:normal"
    )

    $StartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $StartInfo.FileName = $MSBuildExe
    $StartInfo.Arguments = $Arguments

    $StartInfo.EnvironmentVariables.Clear()

    Get-ChildItem -Path env:* | ForEach-Object {
        $StartInfo.EnvironmentVariables.Add($_.Name, $_.Value)
    }

    $StartInfo.UseShellExecute = $false
    $StartInfo.CreateNoWindow = $false
    $StartInfo.RedirectStandardError = $true
    $StartInfo.RedirectStandardOutput = $true

    $Process = New-Object System.Diagnostics.Process
    $Process.StartInfo = $startInfo
    $Process.Start()
    
    $stdout = $Process.StandardOutput.ReadToEnd()
    $stderr = $Process.StandardError.ReadToEnd()
    
    $Process.WaitForExit()

    if($Process.ExitCode -ne 0)
    {
        Write-Host "stdout: $stdout"
        Write-Host "stderr: $stderr"
        Die "Build failed"
    }
}

function VSX 
{
    param(
        [ValidateSet('v142','v143','netcore')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain
    )

    Write-Diagnostic "Starting to build targeting toolchain $Toolchain"

    foreach ($arch in $ARCHES) {
        Msvs "$Toolchain" 'Release' $arch
    }

    Write-Diagnostic "Finished build targeting toolchain $Toolchain"
}

function NugetPackageRestore
{
    if(-not (Test-Path $nuget)) {
        Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
    }

    Write-Diagnostic "Restore Nuget Packages"

    # Restore packages
    if ($IsNetCoreBuild){
        . $nuget restore CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.netcore.config -PackagesDirectory packages
        . $nuget restore CefSharp.BrowserSubprocess.Core\packages.CefSharp.BrowserSubprocess.Core.netcore.config -PackagesDirectory packages
        &msbuild /t:restore CefSharp3.netcore.sln
    }else{
        . $nuget restore $CefSln
    }

	
    # Restore packages
    . $nuget restore CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.config -PackagesDirectory packages
    . $nuget restore CefSharp.BrowserSubprocess.Core\packages.CefSharp.BrowserSubprocess.Core.config -PackagesDirectory packages
}

function Nupkg
{
    if (Test-Path Env:\APPVEYOR_PULL_REQUEST_NUMBER)
    {
        Write-Diagnostic "Pr Number: $env:APPVEYOR_PULL_REQUEST_NUMBER"
        Write-Diagnostic "Skipping Nupkg"
        return
    }
    
    $nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
    if(-not (Test-Path $nuget)) {
        Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
    }

    Write-Diagnostic "Building nuget package"

    $PackageRefAdd = TernaryReturn $IsNetCoreBuild "\PackageReference" ""
    $NetCoreAdd = TernaryReturn $IsNetCoreBuild ".NETCore" ""
    # Build packages
    . $nuget pack "nuget$($PackageRefAdd)\CefSharp.Common$($NetCoreAdd).nuspec" -NoPackageAnalysis -Version $Version -OutputDirectory "nuget$($PackageRefAdd)" -Properties "RedistVersion=$RedistVersion;"
    . $nuget pack "nuget$($PackageRefAdd)\CefSharp.Wpf$($NetCoreAdd).nuspec" -NoPackageAnalysis -Version $Version -OutputDirectory "nuget$($PackageRefAdd)"
    . $nuget pack "nuget$($PackageRefAdd)\CefSharp.OffScreen$($NetCoreAdd).nuspec" -NoPackageAnalysis -Version $Version -OutputDirectory "nuget$($PackageRefAdd)"
    . $nuget pack "nuget$($PackageRefAdd)\CefSharp.WinForms$($NetCoreAdd).nuspec" -NoPackageAnalysis -Version $Version -OutputDirectory "nuget$($PackageRefAdd)"

    # Invoke `AfterBuild` script if available (ie. upload packages to myget)
    if(-not (Test-Path $WorkingDir\AfterBuild.ps1)) {
        return
    }

    . $WorkingDir\AfterBuild.ps1 -Version $Version
}

function DownloadNuget()
{
    $nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
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

function WriteVersionToTransform($transform)
{
    $Filename = Join-Path $WorkingDir $transform
    $Regex = 'codeBase version="(.*?)"';

    $TransformData = Get-Content -Encoding UTF8 $Filename
    $NewString = $TransformData -replace $Regex, "codeBase version=""$AssemblyVersion.0"""

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

NugetPackageRestore

WriteAssemblyVersion
WriteVersionToShfbproj
WriteVersionToAppveyor
WriteVersionToNugetTargets

WriteVersionToManifest "CefSharp.BrowserSubprocess\app.manifest"
WriteVersionToManifest "CefSharp.OffScreen.Example\app.manifest"
WriteVersionToManifest "CefSharp.WinForms.Example\app.manifest"
WriteVersionToManifest "CefSharp.Wpf.Example\app.manifest"

WriteVersionToTransform "NuGet\CefSharp.Common.app.config.x64.transform"
WriteVersionToTransform "NuGet\CefSharp.Common.app.config.x86.transform"

WriteVersionToResourceFile "CefSharp.BrowserSubprocess.Core\Resource.rc"
WriteVersionToResourceFile "CefSharp.Core.Runtime\Resource.rc"

switch -Exact ($Target)
{
    "nupkg-only"
    {
        Nupkg
    }
    "vs2019"
    {
        VSX v142
        Nupkg
    }
    "vs2022"
    {

        VSX v143
        Nupkg
    }
    "netcore31"
    {
        VSX netcore
        Nupkg
    }
}
