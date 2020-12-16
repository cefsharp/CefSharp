$WorkingDir = split-path -parent $MyInvocation.MyCommand.Definition

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

function GenerateRefAssemblySource
{
    $genapiVersion = '6.0.0-beta.20610.4'
	$genapi = Join-Path $WorkingDir \tools\microsoft.dotnet.genapi.$genapiVersion\tools\net472\Microsoft.DotNet.GenAPI.exe
    if(-not (Test-Path $genapi))
    {
		$toolsFolder = Join-Path $WorkingDir \tools
		$genapiNupkg = Join-Path $toolsFolder \microsoft.dotnet.genapi.$genapiVersion.nupkg
        $genapiZip = Join-Path $toolsFolder \microsoft.dotnet.genapi.$genapiVersion.zip
        $client = New-Object System.Net.WebClient;
        #https://www.myget.org/F/cefsharp/api/v2/package/Microsoft.DotNet.GenAPI/6.0.0-beta.20610.4
        $downloadUrl = 'https://www.myget.org/F/cefsharp/api/v2/package/Microsoft.DotNet.GenAPI/' + $genapiVersion
        $client.DownloadFile($downloadUrl, $genapiNupkg);
        #Expand-Archive won't extract a nupkg file, simply rename to zip
        Rename-Item -Path $genapiNupkg -NewName $genapiZip	

		Expand-Archive -LiteralPath $genapiZip -DestinationPath (Join-Path $toolsFolder microsoft.dotnet.genapi.$genapiVersion)
    }

    #.\Microsoft.DotNet.GenAPI.exe C:\projects\CefSharp\CefSharp.Core.Runtime\bin\Win32\Debug\CefSharp.Core.Runtime.dll --lang-version 7.1 --lib-path C:\projects\CefSharp\CefSharp\bin\Debug --out CefSharp.Core.Runtime.cs

    $inputDll = Join-Path $WorkingDir \CefSharp.Core.Runtime\bin\Win32\Release\CefSharp.Core.Runtime.dll
    $outputFile = Join-Path $WorkingDir \CefSharp.Core.Runtime.RefAssembly\CefSharp.Core.Runtime.cs
    $cefSharpDllPath = Join-Path $WorkingDir \CefSharp\bin\Release\
    $mscorlibDllPath = (Get-Item ([System.String].Assembly.Location)).Directory.ToString()
    $libPath = $cefSharpDllPath + ';' + $mscorlibDllPath

    . $genapi $inputDll --lang-version 7.1 --lib-path $libPath --out $outputFile
	Write-Diagnostic "Generated Ref Assembly Source $outputFile"

    #Generates slightly incorrect C#, so just manually fix it.
    ((Get-Content -path $outputFile -Raw) -replace 'public sealed override void Dispose','public void Dispose') | Set-Content -Path $outputFile
}

GenerateRefAssemblySource