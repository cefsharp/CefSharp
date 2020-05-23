param(
    [ValidateSet("vs2015", "vs2017", "vs2019", "nupkg-only", "gitlink")]
    [Parameter(Position = 0)] 
    [string] $Target = "vs2015",
    [Parameter(Position = 1)]
    [string] $Version = "81.3.100",
    [Parameter(Position = 2)]
    [string] $AssemblyVersion = "81.3.100"
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
        [ValidateSet('v140', 'v141', 'v142')]
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
        'v140' {
            if($env:VS140COMNTOOLS -eq $null) {
                Die "Visual Studio 2015 is not installed on your development machine, unable to continue."
            }

            $MSBuildExe = join-path -path (Get-ItemProperty "HKLM:\software\Microsoft\MSBuild\ToolsVersions\14.0").MSBuildToolsPath -childpath "msbuild.exe"
            $MSBuildExe = $MSBuildExe -replace "Framework64", "Framework"
            $VisualStudioVersion = '14.0'
            $VXXCommonTools = Join-Path $env:VS140COMNTOOLS '..\..\vc'
        }
        {($_ -eq 'v141') -or ($_ -eq 'v142')} {
            $VS_VER = 15;
            $VS_OFFICIAL_VER = 2017;
            if ($_ -eq 'v142'){$VS_VER=16;$VS_OFFICIAL_VER=2019;}
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
            $VS2017InstallPath = & $vswherePath -version $versionSearchStr -property installationPath
            
            Write-Diagnostic "$($VS_OFFICIAL_VER)InstallPath: $VS2017InstallPath"
                
            if(-not (Test-Path $VS2017InstallPath))
            {
                Die "Visual Studio $VS_OFFICIAL_VER is not installed on your development machine, unable to continue."
            }
                
            $MSBuildExe = "msbuild.exe"
            $VisualStudioVersion = "$VS_VER.0"
            $VXXCommonTools = Join-Path $VS2017InstallPath VC\Auxiliary\Build
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
        [ValidateSet('v140', 'v141', 'v142')]
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string] $Toolchain
    )

    Write-Diagnostic "Starting to build targeting toolchain $Toolchain"

    Msvs "$Toolchain" 'Release' 'x86'
    Msvs "$Toolchain" 'Release' 'x64'

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
        Write-Diagnostic "Pr Number: $env:APPVEYOR_PULL_REQUEST_NUMBER"
        Write-Diagnostic "Skipping Nupkg"
        return
    }
    
    $nuget = Join-Path $WorkingDir .\nuget\NuGet.exe
    if(-not (Test-Path $nuget)) {
        Die "Please install nuget. More information available at: http://docs.nuget.org/docs/start-here/installing-nuget"
    }

    Write-Diagnostic "Building nuget package"

    # Build old packages
    . $nuget pack nuget\CefSharp.Common.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget -Properties "RedistVersion=$RedistVersion"
    . $nuget pack nuget\CefSharp.Wpf.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget
    . $nuget pack nuget\CefSharp.OffScreen.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget
    . $nuget pack nuget\CefSharp.WinForms.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget
    
    # Build newer style packages
    . $nuget pack nuget\PackageReference\CefSharp.Common.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "RedistVersion=$RedistVersion;Platform=x86;PlatformNative=Win32"
    . $nuget pack nuget\PackageReference\CefSharp.Common.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "RedistVersion=$RedistVersion;Platform=x64;PlatformNative=x64"
    . $nuget pack nuget\PackageReference\CefSharp.OffScreen.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x86"
    . $nuget pack nuget\PackageReference\CefSharp.OffScreen.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x64"
    . $nuget pack nuget\PackageReference\CefSharp.Wpf.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x86"
    . $nuget pack nuget\PackageReference\CefSharp.Wpf.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x64"
    . $nuget pack nuget\PackageReference\CefSharp.WinForms.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x86"
    . $nuget pack nuget\PackageReference\CefSharp.WinForms.win.nuspec -NoPackageAnalysis -Version $Version -OutputDirectory nuget\PackageReference -Properties "Platform=x64"

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
            #Powershell is having problems download GitLink SSL/TLS error, force TLS 1.2
            #https://stackoverflow.com/a/55809878/4583726
            [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::TLS12
            $client = New-Object System.Net.WebClient;
            $client.DownloadFile('https://github.com/GitTools/GitLink/releases/download/2.3.0/GitLink.exe', $gitlink);
        }
    }
    
    Write-Diagnostic "GitLink working dir : $WorkingDir"
    
    # Run GitLink in the workingDir
    . $gitlink $WorkingDir -f CefSharp3.sln -u https://github.com/CefSharp/CefSharp -c Release -p x64 -ignore CefSharp.Example`,CefSharp.Wpf.Example`,CefSharp.OffScreen.Example`,CefSharp.WinForms.Example
    . $gitlink $WorkingDir -f CefSharp3.sln -u https://github.com/CefSharp/CefSharp -c Release -p x86 -ignore CefSharp.Example`,CefSharp.Wpf.Example`,CefSharp.OffScreen.Example`,CefSharp.WinForms.Example
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

Write-Diagnostic "CEF Redist Version = $RedistVersion"

DownloadNuget

NugetPackageRestore

WriteAssemblyVersion
WriteVersionToShfbproj
WriteVersionToAppveyor

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
    "vs2015"
    {
        VSX v140
        UpdateSymbolsWithGitLink
        Nupkg
    }
    "vs2017"
    {
        VSX v141
        UpdateSymbolsWithGitLink
        Nupkg
    }
    "vs2019"
    {
        VSX v142
        UpdateSymbolsWithGitLink
        Nupkg
    }
}
