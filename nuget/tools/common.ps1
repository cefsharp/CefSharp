#param($installPath, $toolsPath, $package, $project)

# Adding/removing <Import> for project.targets

# @see http://nuget.codeplex.com/workitem/847
# @see http://nuget.codeplex.com/discussions/254095
# @see https://gist.github.com/3180872

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
$msbuild = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName) | Select-Object -First 1

$solutionFile = $project.Object.DTE.Solution.FullName
$targetsFile = [System.IO.Path]::Combine($toolsPath, 'project.targets')

$solutionUri = new-object Uri('file://' + $solutionFile + '/..')
$projectUri = new-object Uri('file://' + $project.FullName)
$targetUri = new-object Uri('file://' + $targetsFile)
$relativePath = '$(SolutionDir)\' + $solutionUri.MakeRelativeUri($targetUri).ToString().`
	Replace([System.IO.Path]::AltDirectorySeparatorChar, [System.IO.Path]::DirectorySeparatorChar)
