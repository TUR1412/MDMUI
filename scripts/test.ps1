param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

Write-Host "测试配置: $Configuration"
dotnet test ".\\MDMUI\\MDMUI.Tests\\MDMUI.Tests.csproj" -c $Configuration -v minimal

