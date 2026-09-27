param(
    [string]$projectDir = $PSScriptRoot
)

if (-not [string]::IsNullOrWhiteSpace($projectDir)) {
    $projectDir = $projectDir.Trim().TrimEnd('\', '/', '"', "'")
}
if ([string]::IsNullOrWhiteSpace($projectDir)) {
    $projectDir = $PSScriptRoot
}

$verFile = Join-Path $projectDir "version.txt"
$infoFile = Join-Path $projectDir "Properties\AssemblyInfo.cs"

if (-not (Test-Path $verFile)) {
    [System.IO.File]::WriteAllText($verFile, "1.0.0.0", [System.Text.Encoding]::UTF8)
}

$ver = (Get-Content $verFile -Raw).Trim()
$parts = $ver.Split('.')
if ($parts.Length -ne 4) {
    $parts = @("1", "0", "0", "0")
}
$parts[3] = ([int]$parts[3] + 1).ToString()
$newVer = $parts -join '.'

[System.IO.File]::WriteAllText($verFile, $newVer, [System.Text.Encoding]::UTF8)

if (Test-Path $infoFile) {
    $content = [System.IO.File]::ReadAllText($infoFile, [System.Text.Encoding]::UTF8)
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, '\[assembly:\s*AssemblyVersion\(".*?"\)\]', "[assembly: AssemblyVersion(`"$newVer`")]")
    $content = [System.Text.RegularExpressions.Regex]::Replace($content, '\[assembly:\s*AssemblyFileVersion\(".*?"\)\]', "[assembly: AssemblyFileVersion(`"$newVer`")]")
    [System.IO.File]::WriteAllText($infoFile, $content, [System.Text.Encoding]::UTF8)
}

Write-Host "===> [Release Build] Version auto-incremented to: $newVer" -ForegroundColor Green
