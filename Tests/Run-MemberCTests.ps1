param(
    [string]$BuildTools = 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin'
)
$ErrorActionPreference = 'Stop'
$repoPath = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $repoPath '2011  Semester Project.slnx'
& (Join-Path $BuildTools 'MSBuild.exe') $solutionPath /t:Build /p:Configuration=Debug /verbosity:minimal /nologo
if ($LASTEXITCODE -ne 0) { throw 'Application build failed.' }
$applicationPath = Join-Path $repoPath 'bin\Debug\2011  Semester Project.exe'
$testPath = Join-Path $repoPath 'bin\Debug\MemberCControllerTests.exe'
& (Join-Path $BuildTools 'Roslyn\csc.exe') /nologo /target:exe "/out:$testPath" "/reference:$applicationPath" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll /reference:System.Data.dll (Join-Path $PSScriptRoot 'MemberCControllerTests.cs')
if ($LASTEXITCODE -ne 0) { throw 'Test compilation failed.' }
& $testPath
if ($LASTEXITCODE -ne 0) { throw 'Member C tests failed.' }
