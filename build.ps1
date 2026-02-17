#requires -Version 5

param(
    [ValidateSet("vs2022","vs2019", "nupkg-only", "update-build-version")]
    [Parameter(Position = 0)] 
    [string] $Target = "vs2022",
    [Parameter(Position = 1)]
    [string] $Version = "145.0.240",
    [Parameter(Position = 2)]
    [string] $AssemblyVersion = "145.0.240",
    [Parameter(Position = 3)]
    [ValidateSet("NetFramework", "NetCore")]
    [string] $TargetFramework = "NetFramework",
    [Parameter(Position = 4)]
    [string] $BuildArches = "x86 x64 arm64"
)
Set-StrictMode -version latest;
$ErrorActionPreference = "Stop";

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

function BuildSolution
{
    param(
        [ValidateSet('v142','v143')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain,

        [Parameter(Position = 1, ValueFromPipeline = $true)]
        [ValidateSet('Debug', 'Release')]
        [string] $Configuration,

        [Parameter(Position = 2, ValueFromPipeline = $true)]
        [ValidateSet('x86', 'x64', 'arm64')]
        [string] $Platform,

        [Parameter(Position = 3, ValueFromPipeline = $true)]
        [string] $VisualStudioVersion
    )

    Write-Diagnostic "Begin compiling targeting $Toolchain using configuration $Configuration for platform $Platform"

    $Arch = $Platform
    if (!$IsNetCoreBuild -and $Arch -eq "x86")
    {
        $Arch="win32";
    }

    # Restore Nuget packages
    &msbuild /nologo /verbosity:minimal /t:restore /p:Platform=$Arch /p:Configuration=Release $CefSln

    $Arguments = @(
        "$CefSln",
        "/t:rebuild",
        "/p:VisualStudioVersion=$VisualStudioVersion",
        "/p:Configuration=$Configuration",
        "/p:Platform=$Arch",
        "/verbosity:normal"
    )

    $StartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $StartInfo.FileName = "msbuild.exe"
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

    Write-Diagnostic "Compile succeeded targeting $Toolchain using configuration $Configuration for platform $Platform"
}

function VSX 
{
    param(
        [ValidateSet('v142','v143')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain
    )

    Write-Diagnostic "Starting to build targeting toolchain $Toolchain"

    $VisualStudioVersion = $null
    $VXXCommonTools = $null
    $VS_VER = -1
    $VS_OFFICIAL_VER = -1
    $VS_PRE = ""

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
            $VS_PRE = "-prerelease";
        }
    }

    $versionSearchStr = "[$VS_VER.0," + ($VS_VER+1) + ".0)"

    $ErrorActionPreference="SilentlyContinue"
    $VSInstallPath = & $VSWherePath -version $versionSearchStr -latest -property installationPath $VS_PRE
    $ErrorActionPreference="Stop"
    
    Write-Diagnostic "$($VS_OFFICIAL_VER)InstallPath: $VSInstallPath"
        
    if( -not $VSInstallPath -or -not (Test-Path $VSInstallPath))
    {
        $ErrorActionPreference="SilentlyContinue"
        $VSInstallPath = & $VSwherePath -version $versionSearchStr -property installationPath $VS_PRE -products 'Microsoft.VisualStudio.Product.BuildTools'
        $ErrorActionPreference="Stop"
        Write-Diagnostic "BuildTools $($VS_OFFICIAL_VER)InstallPath: $VSInstallPath"

        if( -not $VSInstallPath -or -not (Test-Path $VSInstallPath))
        {
            Die "Visual Studio $VS_OFFICIAL_VER is not installed on your development machine, unable to continue, ran command: $VSWherePath -version $versionSearchStr -property installationPath"
        }
    }
        
    $VisualStudioVersion = "$VS_VER.0"
    $VXXCommonTools = Join-Path $VSInstallPath VC\Auxiliary\Build

    if ($null -eq $VXXCommonTools -or (-not (Test-Path($VXXCommonTools))))
    {
        Die 'Error unable to find any visual studio environment'
    }

    $VCVarsAll = Join-Path $VXXCommonTools vcvarsall.bat
    if (-not (Test-Path $VCVarsAll))
    {
        Die "Unable to find $VCVarsAll"
    }

    # Only configure build environment once
    if($null -eq $env:CEFSHARP_BUILD_IS_BOOTSTRAPPED)
    {
        $VCVarsAllArch = $ARCHES[0]
        if ($VCVarsAllArch -eq "arm64")
        {
            #TODO: Add support for compiling from an arm64 host
            # Detect host and determine if we are native or cross compile
            # currently only cross compiling arm64 from x64 host
            $VCVarsAllArch = 'x64_arm64'
        }

        Invoke-BatchFile $VCVarsAll $VCVarsAllArch
        $env:CEFSHARP_BUILD_IS_BOOTSTRAPPED = $true
    }

    foreach ($arch in $ARCHES)
    {
        BuildSolution "$Toolchain" 'Release' $arch $VisualStudioVersion     
    }

    Write-Diagnostic "Finished build targeting toolchain $Toolchain"
}

function NugetPackageRestore
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string[]] $ConfigFiles
    )

    Write-Diagnostic "Restore Nuget Packages"

    foreach($file in $ConfigFiles)
    {
        . $nuget restore $file -PackagesDirectory packages
    }
}

function Nupkg
{
    param(
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string[]] $Files
    )

    if (Test-Path Env:\APPVEYOR_PULL_REQUEST_NUMBER)
    {
        Write-Diagnostic "Pr Number: $env:APPVEYOR_PULL_REQUEST_NUMBER"
        Write-Diagnostic "Skipping Nupkg"
        return
    }

    $gitBranch = git rev-parse --abbrev-ref HEAD
    $gitCommit = git rev-parse HEAD

    if (Test-Path Env:\APPVEYOR_REPO_BRANCH) # https://github.com/appveyor/ci/issues/1606
    {
        $gitBranch = $env:APPVEYOR_REPO_BRANCH
    }

    Write-Diagnostic "Building nuget package for $gitCommit on $gitBranch"

    # Build packages
    foreach($file in $Files)
    {
        $filePath = Join-Path $WorkingDir "$NugetPackagePath\$file"
        $tempFile = $filePath + ".backup"
        try
        {
            # We need to rewrite the CefSharp.Common nupkg file if we are building a subset of architectures
            if($file.StartsWith("CefSharp.Common") -and $ARCHES.Count -lt $SupportedArches.Count)
            {                
                Copy-Item $filePath $tempFile
                $removeArches = $SupportedArches | Where-Object {$_ -notin $ARCHES}
                $NupkgXml = [xml](Get-Content ($filePath) -Encoding UTF8)

                foreach($a in $removeArches)
                {
                    $targetFolder = "CefSharp\$a"
                    if($IsNetCoreBuild)
                    {
                        $targetFolder = "runtimes\win-$a"
                    }
                    else
                    {
                        # Remove chromiumembeddedframework.runtime.win* dependency for arches we are not including
                        $depNode =  $NupkgXml.package.metadata.dependencies.group.dependency | Where-Object {$_.Attributes["id"].Value.Equals("chromiumembeddedframework.runtime.win-" + $a) };
                        $depNode.ParentNode.RemoveChild($depNode) | Out-Null
                    }
                    
                    #Remove files
                    $nodes =  $NupkgXml.package.files.file | Where-Object {$_.Attributes["target"].Value.StartsWith($targetFolder) };

                    $nodes | ForEach-Object { $_.ParentNode.RemoveChild($_) } | Out-Null
                }
                
                $NupkgXml.Save($filePath)
            }

            #Only show package analysis for newer packages
            if($IsNetCoreBuild)
            {
                . $nuget pack $filePath -Version $Version -OutputDirectory $NugetPackagePath -Properties "RedistVersion=$RedistVersion;Branch=$gitBranch;CommitSha=$gitCommit;"
            }
            else
            {
                . $nuget pack $filePath -NoPackageAnalysis -Version $Version -OutputDirectory $NugetPackagePath -Properties "RedistVersion=$RedistVersion;Branch=$gitBranch;CommitSha=$gitCommit;"
            }
        }
        finally
        {
            if(Test-Path($tempFile))
            {
                Copy-Item $tempFile $filePath
                Remove-Item $tempFile
            }
        }
    }

    # Invoke `AfterBuild` script if available (ie. upload packages to myget)
    if(-not (Test-Path $WorkingDir\AfterBuild.ps1))
    {
        return
    }

    . $WorkingDir\AfterBuild.ps1 -Version $Version
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

    $RunTimeJsonData = Get-Content -Encoding UTF8 $Filename

    $Regex1  = '" Version=".*"';
    $Replace = '" Version="' + $RedistVersion + '"';
    $NewString = $RunTimeJsonData -replace $Regex1, $Replace

    $Regex1  = '" VersionOverride=".*"';
    $Replace = '" VersionOverride="' + $RedistVersion + '"';
    $NewString = $NewString -replace $Regex1, $Replace
    
    $Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding $False
    [System.IO.File]::WriteAllLines($Filename, $NewString, $Utf8NoBomEncoding)
}

$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition

Write-Diagnostic "pushd $WorkingDir"
Push-Location $WorkingDir

$IsNetCoreBuild = $TargetFramework.ToLower().Contains("netcore")
$ARCHES = [System.Collections.ArrayList]$BuildArches.ToLower().Split(" ");
$CefSln = $null
$NugetPackagePath = $null
$NupkgFiles = $null
$VCXProjPackageConfigFiles = $null
$SupportedArches = [System.Collections.ArrayList]@();

if($IsNetCoreBuild)
{
    $CefSln = Join-Path $WorkingDir 'CefSharp3.netcore.sln'
    $NugetPackagePath = "nuget\PackageReference";
    $NupkgFiles = @('CefSharp.Common.NETCore.nuspec', 'CefSharp.WinForms.NETCore.nuspec', 'CefSharp.Wpf.NETCore.nuspec','CefSharp.OffScreen.NETCore.nuspec', 'CefSharp.Wpf.HwndHost.nuspec')
    $VCXProjPackageConfigFiles = @('CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.netcore.config', 'CefSharp.BrowserSubprocess.Core\packages.CefSharp.BrowserSubprocess.Core.netcore.config');
    $SupportedArches.AddRange(@("x86", "x64", "arm64"));
}
else
{
    $ARCHES.Remove("arm64")
    $CefSln = Join-Path $WorkingDir 'CefSharp3.sln'
    $NugetPackagePath = "nuget";
    $NupkgFiles = @('CefSharp.Common.nuspec', 'CefSharp.WinForms.nuspec', 'CefSharp.Wpf.nuspec', 'CefSharp.OffScreen.nuspec')
    $VCXProjPackageConfigFiles = @('CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.config', 'CefSharp.BrowserSubprocess.Core\packages.CefSharp.BrowserSubprocess.Core.config');
    $SupportedArches.AddRange(@("x86", "x64"));
}

# Extract the current CEF Redist version from the CefSharp.Core.Runtime\packages.CefSharp.Core.Runtime.config file
# Save having to update this file manually Example 3.2704.1418
$CefSharpCorePackagesXml = [xml](Get-Content ($VCXProjPackageConfigFiles[0]))
$RedistVersion = $CefSharpCorePackagesXml.SelectSingleNode("//packages/package[@id='cef.sdk']/@version").value
$nuget = Join-Path $WorkingDir .\nuget\NuGet.exe

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

Write-Diagnostic "CEF Redist Version = $RedistVersion"

DownloadNuget

NugetPackageRestore $VCXProjPackageConfigFiles

$VSWherePath = Join-Path ${env:ProgramFiles} 'Microsoft Visual Studio\Installer\vswhere.exe'

if(-not (Test-Path $VSWherePath))
{
    $VSWherePath = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'
}

#Check if we already have vswhere which is included in newer versions of VS2017/VS2019
if(-not (Test-Path $VSWherePath))
{
    Write-Diagnostic "Downloading VSWhere as no install found at $VSWherePath"
    
    # Check if we already have a local copy and download if required
    $VSWherePath = Join-Path $WorkingDir \vswhere.exe
    
    # TODO: Check hash and download if hash differs
    if(-not (Test-Path $VSWherePath))
    {
        $client = New-Object System.Net.WebClient;
        $client.DownloadFile('https://github.com/Microsoft/vswhere/releases/download/2.2.11/vswhere.exe', $VSWherePath);
    }
}

Write-Diagnostic "VSWhere path $VSWherePath"

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
        Nupkg $NupkgFiles
    }
    "vs2019"
    {
        VSX v142
        Nupkg $NupkgFiles
    }
    "vs2022"
    {
        VSX v143
        Nupkg $NupkgFiles
    }
    "update-build-version"
    {
        Write-Diagnostic "Updated Version to $Version"
        Write-Diagnostic "Updated AssemblyVersion to $AssemblyVersion"
    }
}

Pop-Location
