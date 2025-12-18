param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

Write-Host "构建配置: $Configuration"
dotnet build ".\\MDMUI\\MDMUI.sln" -c $Configuration -v minimal

