$pathToSolution = "./src/CognitiveServices.Explorer/"
$projectName = "CognitiveServices.Explorer.Web"
$repoName = "CognitiveServices.Explorer"

Write-Output "----==== Publish $pathToSolution"
dotnet publish $pathToSolution -c Release -o ./dist/
Write-Output ""

Write-Output "----==== Copy from ./dist/wwwroot"
Copy-Item -Path "./dist/wwwroot/*" -Destination "./" -Recurse -Force
Write-Output ""

$indexFile = "./index.html"
$originalBaseUrlText = "<base href=""/"">";
$targetBaseUrlText = "<base href=""/$repoName/"">";

Write-Output "----==== Replace base href in $indexFile to be /$repoName/"
((Get-Content -path $indexFile -Raw) -replace $originalBaseUrlText, $targetBaseUrlText) | Set-Content -Path $indexFile

$indexFile = "./manifest.json"
$originalBaseUrlText = """start_url"": ""/""";
$targetBaseUrlText = """start_url"": ""/$repoName/""";

Write-Output "----==== Replace manifest start URL in $indexFile to be /$repoName/"
((Get-Content -path $indexFile -Raw) -replace $originalBaseUrlText, $targetBaseUrlText) | Set-Content -Path $indexFile

Write-Output "----==== Delete dist folder"
Remove-Item ./dist/ -Recurse
