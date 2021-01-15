param(
    [ValidateSet("Debug", "Release")]
    [Parameter(Position = 0)] 
    [string] $Configuration = "Release"
)

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
		Write-Diagnostic "Attempting to download $downloadUrl"
        $client.DownloadFile($downloadUrl, $genapiNupkg);
        #Expand-Archive won't extract a nupkg file, simply rename to zip
        Rename-Item -Path $genapiNupkg -NewName $genapiZip	

		Expand-Archive -LiteralPath $genapiZip -DestinationPath (Join-Path $toolsFolder microsoft.dotnet.genapi.$genapiVersion)
    }

    $inputDll = Join-Path $WorkingDir \CefSharp.Core.Runtime\bin\Win32\$Configuration\CefSharp.Core.Runtime.dll
	#This is a little problematic in developmenet as Win32 version might not nessicarily be the current target build
	#as yet I've not found a way to get the solution 
	if(-not (Test-Path $inputDll))
	{
		$inputDll = Join-Path $WorkingDir \CefSharp.Core.Runtime\bin\x64\$Configuration\CefSharp.Core.Runtime.dll
	}
	
	if((Test-Path $inputDll))
	{
		$outputFile = Join-Path $WorkingDir \CefSharp.Core.Runtime.RefAssembly\CefSharp.Core.Runtime.cs
		$cefSharpDllPath = Join-Path $WorkingDir \CefSharp\bin\$Configuration\
		$mscorlibDllPath = (Get-Item ([System.String].Assembly.Location)).Directory.ToString()
		$libPath = $cefSharpDllPath + ';' + $mscorlibDllPath

		. $genapi $inputDll --lang-version 7.1 --lib-path $libPath --out $outputFile

		#Generates slightly incorrect C#, so just manually fix it.
		$outputFileText = ((Get-Content -path $outputFile -Raw) -replace 'public sealed override void Dispose','public void Dispose')
		$outputFileText = $outputFileText.Trim()
		
		#Set-Content puts an empty line at the end, so use WriteAllText instead
		[System.IO.File]::WriteAllText($outputFile, $outputFileText)
		Write-Diagnostic "Generated Ref Assembly Source $outputFile"
	}
	else
	{
		Write-Diagnostic "Unable to Generate Ref Assembly Source, file not found $inputDll"
	}
}

GenerateRefAssemblySource