@echo off
rem Add this section to .csproj:

rem <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="'$(OS)' == 'Windows_NT' and '$(ConfigurationName)' == 'Release'">
rem   <Exec Command="call PostBuild.cmd $(ProjectPath)"/>
rem </Target>

setlocal
rem $(ProjectPath)
if '%1' == '' exit /b 0
rem C:\Repos\Repo\src\project.csproj
set ProjectPath=%1
rem project.csproj
set ProjectFileName=%~nx1
rem project
set ProjectName=%~n1
rem src
for %%i in (.) do set ProjectDirName=%%~nxi
for %%i in (..) do (
 rem C:\Repos\Repo
 set Repo=%%~dpnxi
 rem Repo
 set RepoName=%%~nxi
)
rem Version X.X.X.X
for /f "tokens=3 delims=<>" %%v in ('findstr "<Version>" %ProjectPath%') do set Version=%%v
for /f "tokens=3 delims=<>" %%v in ('findstr "<Description>" %ProjectPath%') do set Description=%%v
rem Date yyyy-mm-dd
set Ymd=%date:~-4%-%date:~3,2%-%date:~0,2%

rem Test build folder
set Test=$TestBuild$

rem Add extra projects to pack their sources here
set AddDirNames=ToKBR-Lib

echo === Pack sources ===

rem set SrcPack=%ProjectName%-v%Version%-(%Ymd%)-src.zip
set SrcPack=%ProjectName%-v%Version%-src.zip

echo Pack sources to %SrcPack%

pushd ..
set Packer="C:\Program Files\7-Zip\7z.exe" a -tzip %SrcPack% -xr!bin -xr!obj
if exist %SrcPack% del %SrcPack%
call :pack %ProjectDirName% %AddDirNames%

echo === Test build ===

"C:\Program Files\7-Zip\7z.exe" x -y %SrcPack% -o%Test%
cd %Test%

call :build_cmd > build.cmd
call :version_txt > version.txt
call :postbuild_cmd > %ProjectDirName%\PostBuild.cmd

"C:\Program Files\7-Zip\7z.exe" a ..\%SrcPack% build.cmd version.txt %ProjectDirName%\PostBuild.cmd

call build.cmd

echo === Pack binaries ===

cd Distr
copy ..\version.txt
rem set BinPack=%ProjectName%-v%Version%-(%Ymd%).zip
set BinPack=%ProjectName%-v%Version%.zip
if exist ..\..\%BinPack% del ..\..\%BinPack%

echo Pack binary application to %BinPack%

"C:\Program Files\7-Zip\7z.exe" a -tzip ..\..\%BinPack%
cd ..\..

echo === Backup ===

set Store=G:\BankApps\AppStore
if not exist %Store% goto :nobackup
copy /y %SrcPack% %Store%
copy /y %BinPack% %Store%
:nobackup

echo === All done ===

rd /s /q %Test%
popd
endlocal
exit /b 0

:pack
if '%1' == '' goto :eof

echo === Pack %1 ===

%Packer% -r %1\*.cs %1\*.resx
%Packer% %1\*.csproj %1\*.json %1\*.cmd
shift
goto pack

:lower
echo>%Temp%\%2
for /f %%f in ('dir /b/l %Temp%\%2') do set %1=%%f
del %Temp%\%2
goto :eof

:build_cmd
echo dotnet publish %ProjectDirName%\%ProjectFileName% -o Distr
goto :eof

:version_txt
call :lower RepoLName %RepoName%
echo %ProjectName% v%Ver% (%Ymd%)
echo.
echo https://github.com/diev/%RepoName%
echo https://gitflic.ru/project/diev/%RepoLName%
goto :eof

:postbuild_cmd
echo exit /b 0
goto :eof
