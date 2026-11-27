#requires -Version 7.0
# Non-interactive, cross-platform CEF upgrade script for coding agents.
[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidatePattern('^[0-9A-Za-z][0-9A-Za-z.+-]*$')]
    [string] $CefVersion,

    [Parameter(Position = 1)]
    [ValidatePattern('^[0-9A-Za-z][0-9A-Za-z.+-]*$')]
    [string] $CefSharpVersion = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$parsedCefVersion = $null
if (-not [Version]::TryParse($CefVersion, [ref] $parsedCefVersion)) {
    throw "CefVersion must be a valid numeric version: $CefVersion"
}
$CefVersion = $parsedCefVersion.ToString()

if ([string]::IsNullOrWhiteSpace($CefSharpVersion)) {
    $CefSharpVersion = $CefVersion + '0'
}

$parsedCefSharpVersion = $null
if (-not [Version]::TryParse($CefSharpVersion, [ref] $parsedCefSharpVersion)) {
    throw "CefSharpVersion must be a valid numeric version: $CefSharpVersion"
}
$CefSharpVersion = $parsedCefSharpVersion.ToString()

$WorkingDir = $PSScriptRoot
$Updates = [System.Collections.Generic.Dictionary[string,string]]::new([System.StringComparer]::Ordinal)

function Get-UpdatedContent {
    param([Parameter(Mandatory)][string] $Path)

    if (-not $Updates.ContainsKey($Path)) {
        if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
            throw "Required upgrade file not found: $Path"
        }

        $Updates[$Path] = [System.IO.File]::ReadAllText($Path)
    }

    return $Updates[$Path]
}

function Set-RegexUpdate {
    param(
        [Parameter(Mandatory)][string] $RelativePath,
        [Parameter(Mandatory)][string] $Pattern,
        [Parameter(Mandatory)][string] $Replacement,
        [int] $MinimumMatches = 1
    )

    $path = Join-Path $WorkingDir $RelativePath
    $content = Get-UpdatedContent $path
    $matches = [regex]::Matches($content, $Pattern)
    if ($matches.Count -lt $MinimumMatches) {
        throw "Expected at least $MinimumMatches match(es) for '$Pattern' in $RelativePath; found $($matches.Count). No files have been written."
    }

    $Updates[$path] = [regex]::Replace($content, $Pattern, $Replacement)
}

function Remove-EnsureNuGetBuildImportsTarget {
    param([Parameter(Mandatory)][string] $RelativePath)

    $path = Join-Path $WorkingDir $RelativePath
    $content = Get-UpdatedContent $path
    $pattern = '(?s)<Target\b(?=[^>]*\bName="EnsureNuGetPackageBuildImports")[^>]*(?:/>|>.*?</Target>)'
    $Updates[$path] = [regex]::Replace($content, $pattern, '')
}

# Keep the four packages.config files and the corresponding project imports in sync.
$configFiles = @(
    'CefSharp.Core.Runtime/packages.CefSharp.Core.Runtime.config',
    'CefSharp.Core.Runtime/packages.CefSharp.Core.Runtime.netcore.config',
    'CefSharp.BrowserSubprocess.Core/packages.CefSharp.BrowserSubprocess.Core.config',
    'CefSharp.BrowserSubprocess.Core/packages.CefSharp.BrowserSubprocess.Core.netcore.config'
)
foreach ($file in $configFiles) {
    Set-RegexUpdate $file '(<package\b(?=[^>]*\bid="cef\.sdk")[^>]*\bversion=")[^"]+("[\s\S]*?/?>)' ('${1}' + $CefVersion + '${2}')
}

$projectFiles = @(
    'CefSharp.Core.Runtime/CefSharp.Core.Runtime.vcxproj',
    'CefSharp.Core.Runtime/CefSharp.Core.Runtime.netcore.vcxproj',
    'CefSharp.BrowserSubprocess.Core/CefSharp.BrowserSubprocess.Core.vcxproj',
    'CefSharp.BrowserSubprocess.Core/CefSharp.BrowserSubprocess.Core.netcore.vcxproj'
)
foreach ($file in $projectFiles) {
    Set-RegexUpdate $file '(packages\\cef\.sdk\.)[^"\\]+' ('${1}' + $CefVersion) 2
    Remove-EnsureNuGetBuildImportsTarget $file
}

# Update the Chromium runtime package references in the same example/test projects
# as UpdateNugetPackages.ps1.
$csprojFiles = @(
    'CefSharp.WinForms.Example/CefSharp.WinForms.Example.netcore.csproj',
    'CefSharp.Wpf.Example/CefSharp.Wpf.Example.netcore.csproj',
    'CefSharp.OffScreen.Example/CefSharp.OffScreen.Example.netcore.csproj',
    'CefSharp.Test/CefSharp.Test.netcore.csproj',
    'CefSharp.WinForms.Example/CefSharp.WinForms.Example.csproj',
    'CefSharp.Wpf.Example/CefSharp.Wpf.Example.csproj',
    'CefSharp.OffScreen.Example/CefSharp.OffScreen.Example.csproj',
    'CefSharp.Test/CefSharp.Test.csproj',
    'CefSharp.Wpf.HwndHost.Example/CefSharp.Wpf.HwndHost.Example.netcore.csproj',
    'CefSharp.Wpf.HwndHost.Example/CefSharp.Wpf.HwndHost.Example.csproj'
)
foreach ($file in $csprojFiles) {
    Set-RegexUpdate $file '(<PackageReference\b(?=[^>]*\bInclude="chromiumembeddedframework\.runtime")[^>]*\bVersion=")[^"]+("[^>]*/?>)' ('${1}' + $CefVersion + '${2}')
}

# Match the version defaults previously updated by UpdateNugetPackages.ps1.
Set-RegexUpdate 'UpdateNugetPackages.ps1' '(?m)(^\s*\[string\]\s+\$CefVersion\s*=\s*")[^"]+("\s*,?\s*$)' ('${1}' + $CefVersion + '${2}')
Set-RegexUpdate 'build.ps1' '(?m)(^\s*\[string\]\s+\$Version\s*=\s*")[^"]+("\s*,?\s*$)' ('${1}' + $CefSharpVersion + '${2}')
Set-RegexUpdate 'build.ps1' '(?m)(^\s*\[string\]\s+\$AssemblyVersion\s*=\s*")[^"]+("\s*,?\s*$)' ('${1}' + $CefSharpVersion + '${2}')

# Reproduce the metadata/version updates normally performed by build.ps1's
# update-build-version target, without invoking its Windows-only build setup.
$assemblyVersion = $parsedCefSharpVersion.ToString()
$assemblyFileBuild = if ($parsedCefSharpVersion.Build -ge 0) { $parsedCefSharpVersion.Build } else { 0 }
$assemblyFileRevision = if ($parsedCefSharpVersion.Revision -ge 0) { $parsedCefSharpVersion.Revision } else { 0 }
$assemblyFileVersion = [Version]::new(
    $parsedCefSharpVersion.Major,
    $parsedCefSharpVersion.Minor,
    $assemblyFileBuild,
    $assemblyFileRevision
).ToString()
Set-RegexUpdate 'CefSharp/Properties/AssemblyInfo.cs' '(public const string AssemblyVersion = ")[^"]+(")' ('${1}' + $assemblyVersion + '${2}')
Set-RegexUpdate 'CefSharp/Properties/AssemblyInfo.cs' '(public const string AssemblyFileVersion = ")[^"]+(")' ('${1}' + $assemblyFileVersion + '${2}')

foreach ($file in @(
    'CefSharp.BrowserSubprocess/app.manifest',
    'CefSharp.OffScreen.Example/app.manifest',
    'CefSharp.WinForms.Example/app.manifest',
    'CefSharp.Wpf.Example/app.manifest'
)) {
    Set-RegexUpdate $file '(assemblyIdentity version=")[^"]+(")' ('${1}' + $assemblyFileVersion + '${2}')
}

foreach ($file in @(
    'NuGet/CefSharp.Common.app.config.x64.transform',
    'NuGet/CefSharp.Common.app.config.x86.transform'
)) {
    Set-RegexUpdate $file '(codeBase version=")[^"]+(")' ('${1}' + $assemblyFileVersion + '${2}')
}

foreach ($file in @(
    'CefSharp.BrowserSubprocess.Core/Resource.rc',
    'CefSharp.Core.Runtime/Resource.rc'
)) {
    Set-RegexUpdate $file '(?m)(^\s*(?:FILE|PRODUCT)?VERSION\s+)[^\r\n]+' ('${1}' + ($assemblyVersion -replace '\.', ',')) 2
    Set-RegexUpdate $file '(Version", ")[^"]*(")' ('${1}' + $assemblyVersion + '${2}')
}

Set-RegexUpdate 'CefSharp.shfbproj' '(<HelpFileVersion>)[^<]*(</HelpFileVersion>)' ('${1}' + $assemblyVersion + '${2}')
Set-RegexUpdate 'CefSharp.shfbproj' '(<HeaderText>Version )[^<]*(</HeaderText>)' ('${1}' + $assemblyVersion + '${2}')
Set-RegexUpdate 'appveyor.yml' '(?m)(^version:\s*)[^\r\n]*-CI\{build\}' ('${1}' + $assemblyVersion + '-CI{build}')

# These package versions are embedded in the targets file in both attributes
# (Version and VersionOverride); update them from the new cef.sdk version.
Set-RegexUpdate 'NuGet/PackageReference/CefSharp.Common.NETCore.targets' '(" Version=")[^"]*(")' ('${1}' + $CefVersion + '${2}')
Set-RegexUpdate 'NuGet/PackageReference/CefSharp.Common.NETCore.targets' '(" VersionOverride=")[^"]*(")' ('${1}' + $CefVersion + '${2}')

# Validate every changed XML/MSBuild file before writing anything.
foreach ($path in $Updates.Keys) {
    if ([IO.Path]::GetExtension($path) -in @('.config', '.vcxproj', '.csproj', '.manifest', '.transform', '.shfbproj', '.targets')) {
        $document = [System.Xml.XmlDocument]::new()
        $document.LoadXml($Updates[$path])
    }
}

foreach ($path in $Updates.Keys) {
    if (-not $PSCmdlet.ShouldProcess($path, "Write Cef $CefVersion / CefSharp $CefSharpVersion upgrade changes")) {
        continue
    }

    $bytes = [System.IO.File]::ReadAllBytes($path)
    $hasUtf8Bom = $bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF
    $encoding = [System.Text.UTF8Encoding]::new($hasUtf8Bom)
    [System.IO.File]::WriteAllText($path, $Updates[$path], $encoding)
}

Write-Host "Prepared Cef $CefVersion and CefSharp $CefSharpVersion version updates. Review the diff, then build/test on Windows CI."
