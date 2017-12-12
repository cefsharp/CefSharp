param(
    [ValidateSet("vs2013", "vs2015", "nupkg-only", "gitlink")]
    [Parameter(Position = 0)]
    [string] $Target = "vs2013",
    [Parameter(Position = 1)]
    [string] $Version = "59.0.0",
    [Parameter(Position = 2)]
    [string] $AssemblyVersion = "59.0.0"
)

$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition
$CefSln = Join-Path $WorkingDir 'CefSharp3.sln'

# Extract the current CEF Redist version from the CefSharp.Core\packages.config file
# Save having to update this file manually Example 3.2704.1418
$CefSharpCorePackagesXml = [xml](Get-Content (Join-Path $WorkingDir 'CefSharp.Core\Packages.config'))
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

# if (Test-Path Env:\APPVEYOR_BUILD_VERSION)
# {
#     $Version = $env:APPVEYOR_BUILD_VERSION
# }

# Temporary hack since we need this to be right in both the Version and the Dependencies for the packages to be really installable.
$Version = '62.0.0-proprietary-codecs2'

if ($env:APPVEYOR_REPO_TAG -eq "True")
{
    $Version = "$env:APPVEYOR_REPO_TAG_NAME".Substring(1)  # trim leading "v"
    Write-Diagnostic "Setting version based on tag to $Version"
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

function Msvs
{
    param(
        [ValidateSet('v120', 'v140')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain,

        [Parameter(Position = 1, ValueFromPipeline = $true)]
        [ValidateSet('Debug', 'Release')]
        [string] $Configuration,

        [Parameter(Position = 2, ValueFromPipeline = $true)]
        [ValidateSet('x86', 'x64')]
        [string] $Platform
    )

    Write-Diagnostic "Targeting $Toolchain using configuration $Configuration on platform $Platform"

    $VisualStudioVersion = $null
    $VXXCommonTools = $null

    switch -Exact ($Toolchain) {
        'v120' {
            $MSBuildExe = join-path -path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\12.0").MSBuildToolsPath -childpath "msbuild.exe"
            $MSBuildExe = $MSBuildExe -replace "Framework64", "Framework"
            $VisualStudioVersion = '12.0'
            $VXXCommonTools = Join-Path $env:VS120COMNTOOLS '..\..\vc'
        }
        'v140' {
            $MSBuildExe = join-path -path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\14.0").MSBuildToolsPath -childpath "msbuild.exe"
            $MSBuildExe = $MSBuildExe -replace "Framework64", "Framework"
            $VisualStudioVersion = '14.0'
            $VXXCommonTools = Join-Path $env:VS140COMNTOOLS '..\..\vc'
        }
    }

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

    $Arch = TernaryReturn ($Platform -eq 'x64') 'x64' 'win32'

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
        [ValidateSet('v120', 'v140')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain
    )

    if($Toolchain -eq 'v120' -and $env:VS120COMNTOOLS -eq $null) {
        Warn "Toolchain $Toolchain is not installed on your development machine, skipping build."
        Return
    }

    if($Toolchain -eq 'v140' -and $env:VS140COMNTOOLS -eq $null) {
        Warn "Toolchain $Toolchain is not installed on your development machine, skipping build."
        Return
    }

    Write-Diagnostic "Starting to build targeting toolchain $Toolchain"

    Msvs "$Toolchain" 'Release' 'x86'

    # TODO: reenable if/when we have x64 packages we can use.
    # Msvs "$Toolchain" 'Release' 'x64'

    Write-Diagnostic "Finished build targeting toolchain $Toolchain"
}

function NugetPackageRestore
{
    $nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
    if(-not (Test-Path $nuget)) {
        Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
    }

    Write-Diagnostic "Restore Nuget Packages"

    # Restore packages
    . $nuget restore $CefSln
}

function Nupkg
{
    if (Test-Path Env:\APPVEYOR_PULL_REQUEST_NUMBER)
    {
        Write-Diagnostic "PR Number: $env:APPVEYOR_PULL_REQUEST_NUMBER"
        Write-Diagnostic "Skipping Nupkg"
        return
    }

    $nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
    if(-not (Test-Path $nuget)) {
        Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
    }

    Write-Diagnostic "Building nuget package"

    # Build packages
    . $nuget pack nuget\CefSharp.Common.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget -Properties "RedistVersion=$RedistVersion"
    . $nuget pack nuget\CefSharp.Wpf.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget
    . $nuget pack nuget\CefSharp.OffScreen.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget
    . $nuget pack nuget\CefSharp.WinForms.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget

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
}

function UpdateSymbolsWithGitLink()
{
    $gitlink = "GitLink.exe"

    #Check for GitLink
    if ((Get-Command $gitlink -ErrorAction SilentlyContinue) -eq $null)
    {
        #Download if not on path and not in Nuget folder (TODO: change to different folder)
        $gitlink = Join-Path $WorkingDir .\nuget\GitLink.exe
        if(-not (Test-Path $gitlink))
        {
            Write-Diagnostic "Downloading GitLink"
            $client = New-Object System.Net.WebClient;
            $client.DownloadFile('https://github.com/GitTools/GitLink/releases/download/2.3.0/GitLink.exe', $gitlink);
        }
    }

    Write-Diagnostic "GitLink working dir : $WorkingDir"

    # Run GitLink in the workingDir
    . $gitlink $WorkingDir -f CefSharp3.sln -u https://github.com/CefSharp/CefSharp -c Release -p x64 -ignore CefSharp.Example,CefSharp.Wpf.Example,CefSharp.OffScreen.Example,CefSharp.WinForms.Example
    . $gitlink $WorkingDir -f CefSharp3.sln -u https://github.com/CefSharp/CefSharp -c Release -p x86 -ignore CefSharp.Example,CefSharp.Wpf.Example,CefSharp.OffScreen.Example,CefSharp.WinForms.Example
}

function WriteAssemblyVersion
{
    param()

    $Filename = Join-Path $WorkingDir CefSharp\Properties\AssemblyInfo.cs
    $Regex = 'public const string AssemblyVersion = "(.*)"';
    $Regex2 = 'public const string AssemblyFileVersion = "(.*)"'

    $AssemblyInfo = Get-Content $Filename
    $NewString = $AssemblyInfo -replace $Regex, "public const string AssemblyVersion = ""$AssemblyVersion"""
    $NewString = $NewString -replace $Regex2, "public const string AssemblyFileVersion = ""$AssemblyVersion.0"""

    $NewString | Set-Content $Filename -Encoding UTF8
}

function WriteVersionToManifest($manifest)
{
    $Filename = Join-Path $WorkingDir $manifest
    $Regex = 'assemblyIdentity version="(.*?)"';

    $ManifestData = Get-Content $Filename
    $NewString = $ManifestData -replace $Regex, "assemblyIdentity version=""$AssemblyVersion.0"""

    $NewString | Set-Content $Filename -Encoding UTF8
}

function WriteVersionToResourceFile($resourceFile)
{
    $Filename = Join-Path $WorkingDir $resourceFile
    $Regex1 = 'VERSION .*';
    $Regex2 = 'Version", ".*?"';

    $ResourceData = Get-Content $Filename
    $NewString = $ResourceData -replace $Regex1, "VERSION $AssemblyVersion"
    $NewString = $NewString -replace $Regex2, "Version"", ""$AssemblyVersion"""

    $NewString | Set-Content $Filename -Encoding UTF8
}

Write-Diagnostic "CEF Redist Version = $RedistVersion"

DownloadNuget

NugetPackageRestore

WriteAssemblyVersion

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
    "gitlink"
    {
        UpdateSymbolsWithGitLink
    }
    "vs2013"
    {
        VSX v120
        UpdateSymbolsWithGitLink
        Nupkg
    }
    "vs2015"
    {
        VSX v140
        UpdateSymbolsWithGitLink
        Nupkg
    }
}
