$pathToSolution = "./src/CognitiveServices.Explorer/"
$projectName = "CognitiveServices.Explorer.Web"
$repoName = "CognitiveServices.Explorer"

Write-Output "----==== Publish $pathToSolution"
dotnet publish $pathToSolution -c Release -o ./dist/

Write-Output "----==== Copy from ./dist/$projectName/dist"
Copy-Item -Path "./dist/$projectName/dist" -Destination "." -Recurse -Force

$indexFile = "./index.html"
$originalBaseUrlText = "<base href=""/"">";
$targetBaseUrlText = "<base href=""/$repoName/"">";

Write-Output "----==== Replace base href in $indexFile to be /$repoName/"
((Get-Content -path $indexFile -Raw) -replace $originalBaseUrlText, $targetBaseUrlText) | Set-Content -Path $indexFile

Write-Output "----==== Delete dist folder"
Remove-Item ./dist/ -Recurse
