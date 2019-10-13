$pathToSolution = "./src/CognitiveServices.Explorer/"
$projectName = "CognitiveServices.Explorer.Web"
$repoName = "CognitiveServices.Explorer"

Write-Output "----==== Publish $pathToSolution"
dotnet publish $pathToSolution -c Release -o ./dist/
Write-Output ""

Write-Output "----==== Copy from ./dist/$projectName/dist"
Copy-Item -Path "./dist/$projectName/dist/*" -Destination "./" -Recurse -Force
Write-Output ""

Write-Output "----==== Delete dist folder"
Remove-Item ./dist/ -Recurse
