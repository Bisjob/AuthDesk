param(
  [string]$Configuration = "Release",
  [string]$Runtime = "win-x64"
)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $root

Write-Host "Publishing AuthDesk project..."
$publishDir = Join-Path $root "artifacts\publish"
Remove-Item -Recurse -Force $publishDir -ErrorAction SilentlyContinue
dotnet publish "AuthDesk.App\AuthDesk.App.csproj" -c $Configuration -r $Runtime --self-contained false -o $publishDir
if ($LASTEXITCODE -ne 0) { Exit $LASTEXITCODE }

Write-Host "Preparing package folder..."
$packageDir = "AuthDesk.Packaging.Net\Package"
Remove-Item -Recurse -Force $packageDir -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $packageDir | Out-Null

Write-Host "Copying published outputs to package folder..."
Copy-Item -Path (Join-Path $publishDir "*") -Destination $packageDir -Recurse -Force

Write-Host "Package created at: $packageDir"
Pop-Location
