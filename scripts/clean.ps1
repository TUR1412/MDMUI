param(
    [switch]$IncludeVsCache
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$repoRootResolved = (Resolve-Path $repoRoot).Path

function Remove-SafePath {
    param([Parameter(Mandatory = $true)][string]$Path)

    if (-not (Test-Path $Path)) {
        return
    }

    $resolved = (Resolve-Path $Path).Path
    if (-not $resolved.StartsWith($repoRootResolved)) {
        throw "拒绝删除：目标不在仓库根目录内：$resolved"
    }

    Remove-Item -LiteralPath $resolved -Recurse -Force
}

Write-Host "仓库根目录: $repoRootResolved"

Get-ChildItem -Path $repoRootResolved -Directory -Recurse -Force |
    Where-Object { $_.Name -in @('bin', 'obj') } |
    ForEach-Object { Remove-SafePath $_.FullName }

if ($IncludeVsCache) {
    Remove-SafePath (Join-Path $repoRootResolved '.vs')
}

Write-Host "清理完成。"

