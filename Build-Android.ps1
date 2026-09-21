param(
    [ValidateSet('dev', 'release')][string]$Mode = 'dev',
    [string]$ProjectPath = 'C:\Dev\Cloudora-codex',
    [string]$UnityPath = 'C:\Program Files\Unity\Hub\Editor\6000.5.9f1\Editor\Unity.exe'
)

$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $UnityPath -PathType Leaf)) { throw "Unity not found: $UnityPath" }
if (-not (Test-Path -LiteralPath (Join-Path $ProjectPath 'Assets') -PathType Container)) { throw "Unity project not found: $ProjectPath" }
if ($ProjectPath -match '[^\x00-\x7F]' -or $ProjectPath.Contains(' ')) {
    throw 'Unity Android build requires an ASCII-only project alias with no spaces (for example C:\Dev\Cloudora-codex).'
}

$method = if ($Mode -eq 'release') { 'Cloudora.Editor.CloudoraBuildTools.BuildReleaseAab' } else { 'Cloudora.Editor.CloudoraBuildTools.BuildDevelopmentApk' }
$logPath = Join-Path $ProjectPath "Logs\android-$Mode-build.log"
New-Item -ItemType Directory -Force -Path (Split-Path $logPath) | Out-Null

# Java 17 NIO sockets fail when the Windows profile temp path contains non-ASCII characters.
# An ASCII TEMP/TMP path avoids inherited JVM notices that Android Gradle Plugin treats as errors.
$previousTemp = $env:TEMP
$previousTmp = $env:TMP
try {
    $env:TEMP = Join-Path $ProjectPath 'Library'
    $env:TMP = $env:TEMP
    $arguments = @('-batchmode', '-nographics', '-quit', '-projectPath', $ProjectPath, '-executeMethod', $method, '-logFile', $logPath)
    $process = Start-Process -FilePath $UnityPath -ArgumentList $arguments -WindowStyle Hidden -Wait -PassThru
    if ($process.ExitCode -ne 0) { throw "Unity exited with code $($process.ExitCode). See $logPath" }
    if (-not (Select-String -LiteralPath $logPath -Pattern 'CLOUDORA_BUILD result=Succeeded' -Quiet)) {
        throw "Unity did not report a successful build. See $logPath"
    }
    Write-Output "Unity Android $Mode build succeeded. Log: $logPath"
} finally {
    $env:TEMP = $previousTemp
    $env:TMP = $previousTmp
}
