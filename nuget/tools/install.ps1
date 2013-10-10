param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath "common.ps1")

Write-Host "Adding import of " $relativePath
$importElement = $msbuild.Xml.AddImport($relativePath)
$importElement.Condition = "Exists('" + $relativePath + "')"

$project.Save()
