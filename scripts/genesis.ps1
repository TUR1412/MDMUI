param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',

    [switch]$Commit,
    [switch]$Push
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
Set-Location $repoRoot

Write-Host "GENESIS: build => $Configuration"
dotnet build ".\\MDMUI\\MDMUI.sln" -c $Configuration -v minimal

if ($Commit) {
    $status = git status --porcelain
    if ([string]::IsNullOrWhiteSpace($status)) {
        Write-Host "工作区无变更，跳过提交。"
    } else {
        git add -A
        git commit -m "feat(GOD-MODE): Ultimate Evolution - Quark-level UI & Arch Upgrade"
    }
}

if ($Push) {
    # 安全默认：不做 --force，不做删除目录；如需改写历史请手动执行并自行承担风险。
    git push
}

