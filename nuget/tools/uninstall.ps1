param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath "common.ps1")

$import = $msbuild.Xml.Imports | where { $_.Project -eq $relativePath } | select -first 1
if ($import -ne $null) {
	Write-Host "Removing import of " $relativePath
	$msbuild.Xml.RemoveChild($import)
	$project.Save()
}
