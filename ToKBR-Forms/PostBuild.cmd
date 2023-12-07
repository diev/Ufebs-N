@echo off
rem Add this section to .csproj:

rem <Target Name="PostBuild" AfterTargets="PostBuildEvent" Condition="'$(OS)' == 'Windows_NT' and '$(ConfigurationName)' == 'Release'">
rem   <Exec Command="call PostBuild.cmd $(ProjectPath)"/>
rem </Target>

setlocal
rem $(ProjectPath)
if '%1' == '' exit /b 0
rem C:\Repos\repo\src\project.csproj
set ProjectPath=%1
rem project.csproj
set ProjectFileName=%~nx1
rem project
set ProjectName=%~n1
rem src
for %%i in (.) do set ProjectDirName=%%~nxi
rem C:\Repos\repo
for %%i in (..) do set Repo=%%~dpnxi
rem Version X.X.X.X
for /f "tokens=3 delims=<>" %%v in ('findstr "<AssemblyVersion>" %ProjectPath%') do set Ver=%%v
rem Date yyyy-mm-dd
set Ymd=%date:~-4%-%date:~3,2%-%date:~0,2%

rem Test build folder
set Test=$TestBuild$

rem Add extra projects to pack their sources here
set AddDirNames=ToKBR-Lib

echo === Pack sources ===

set SrcPack=%ProjectName%-v%Ver%-(%Ymd%)-src.zip

echo Pack sources to %SrcPack%

pushd ..
set Packer="C:\Program Files\7-Zip\7z.exe" a -tzip %SrcPack% -xr!bin -xr!obj
if exist %SrcPack% del %SrcPack%
call :pack %ProjectDirName% %AddDirNames%

echo === Test build ===

"C:\Program Files\7-Zip\7z.exe" x -y %SrcPack% -o%Test%
cd %Test%

set t1=build.cmd
echo dotnet publish %ProjectDirName%\%ProjectFileName% -o Distr>%t1%

set t2=version.txt
echo %ProjectName% v%Ver% (%Ymd%)>%t2%
echo.>>%t2%
echo https://github.com/diev>>%t2%
echo https://gitflic.ru/user/diev>>%t2%

rem Replace this file with a dummy
set t3=%ProjectDirName%\PostBuild.cmd
echo exit /b 0 >%t3%

"C:\Program Files\7-Zip\7z.exe" a ..\%SrcPack% %t1% %t2% %t3%

call build.cmd

echo === Pack binaries ===

cd Distr
copy ..\version.txt
set BinPack=%ProjectName%-v%Ver%-(%Ymd%).zip
if exist ..\..\%BinPack% del ..\..\%BinPack%

echo Pack binary application to %BinPack%

"C:\Program Files\7-Zip\7z.exe" a -tzip ..\..\%BinPack%
cd ..\..

echo === Backup ===

set Store=G:\BankApps\%ProjectName%
if exist %Store% copy /y %SrcPack% %Store%
if exist %Store% copy /y %BinPack% %Store%

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
