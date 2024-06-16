param (
    [string]$oldstring,
    [string]$newstring
)

if ($PSCmdlet.MyInvocation.BoundParameters.Count -ne 2) {
    Write-Host "Usage: .\script.ps1 <old string> <new string>"
    exit 1
}

$directory = "."

# Replace string in file and folder names
Get-ChildItem -Path $directory -Recurse -Force -Exclude .git | Where-Object { $_.Name -like "*$oldstring*" } | ForEach-Object {
    $newName = $_.Name -replace [regex]::Escape($oldstring), $newstring
    Rename-Item -Path $_.FullName -NewName $newName -Force
}

# Replace string in file contents
Get-ChildItem -Path $directory -Recurse -File -Force -Exclude .git | ForEach-Object {
    (Get-Content -Path $_.FullName) -replace [regex]::Escape($oldstring), $newstring | Set-Content -Path $_.FullName
}
