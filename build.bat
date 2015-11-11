@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)
 
set version=1.0.0
if not "%PackageVersion%" == "" (
   set version=%PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

REM Package restore
call %NuGet% restore Gallifrey.SharedKernel\packages.config -OutputDirectory %cd%\packages -NonInteractive
call %NuGet% restore Gallifrey.Infrastructure\packages.config -OutputDirectory %cd%\packages -NonInteractive
call %NuGet% restore Gallifrey.Persistence\packages.config -OutputDirectory %cd%\packages -NonInteractive
call %NuGet% restore Gallifrey.RestApiController\packages.config -OutputDirectory %cd%\packages -NonInteractive
call %NuGet% restore FunctonalTest\packages.config -OutputDirectory %cd%\packages -NonInteractive

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Gallifrey.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false

mkdir Build
mkdir Build\lib
mkdir Build\lib\net40

%nuget% pack "Gallifrey.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -Version %version% -p Configuration="%config%"
