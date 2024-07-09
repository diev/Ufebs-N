@echo off
for %%i in (.) do (
 set repo=%%~nxi
)
set store=G:\BankApps\AppStore
rem ----------------------------------------------

set cli=ToKBR
set gui=ToKBR-Forms

rem ----------------------------------------------
call :build %cli%
rem ----------------------------------------------
rem AppVeyor
if /%CliAppZip%/==// (set pack=%app%-v%version%.zip) else (set pack=%CliAppZip%)
rem ----------------------------------------------
call :pack %cli%
rem ----------------------------------------------

rem ----------------------------------------------
call :build %gui%
rem ----------------------------------------------
rem AppVeyor
if /%GuiAppZip%/==// (set pack=%app%-v%version%.zip) else (set pack=%GuiAppZip%)
rem ----------------------------------------------
call :pack %gui%

rem ----------------------------------------------
exit /b 0

:build %1
echo build %1
for %%i in (%1\*.csproj) do (
 set prj=%%~dpnxi
 set app=%%~ni
)
if exist bin rd /s /q bin
rem ----------------------------------------------
rem * Build an app with many dlls (default)
rem dotnet publish %prj% -o bin

rem * Build a single-file app when NET Desktop runtime required 
dotnet publish %prj% -o bin -r win-x64 -p:PublishSingleFile=true --no-self-contained

rem * Build a single-file app when no runtime required
rem dotnet publish %prj% -o bin -r win-x64 -p:PublishSingleFile=true
rem ----------------------------------------------
for /f "tokens=3 delims=<>" %%v in ('findstr "<Version>" %prj%') do set version=%%v
for /f "tokens=3 delims=<>" %%v in ('findstr "<Description>" %prj%') do set description=%%v
rem ----------------------------------------------
call :version_txt > bin\version.txt
rem ----------------------------------------------
goto :eof

:pack
if exist %pack% del %pack%
"C:\Program Files\7-Zip\7z.exe" a %pack% LICENSE *.md *.sln *.cmd bin\
"C:\Program Files\7-Zip\7z.exe" a %pack% -r -x!.* -x!bin -x!obj -x!PublishProfiles -x!*.user %1\
"C:\Program Files\7-Zip\7z.exe" a %pack% -r -x!.* -x!bin -x!obj -x!PublishProfiles -x!*.user ToKBR-Lib\
rd /s /q bin
rem ----------------------------------------------
if exist %store% copy /y %pack% %store%
goto :eof

:lower
rem ----------------------------------------------
echo>%Temp%\%2
for /f %%f in ('dir /b/l %Temp%\%2') do set %1=%%f
del %Temp%\%2
rem ----------------------------------------------
goto :eof

:version_txt
rem ----------------------------------------------
for /f "tokens=3,3" %%a in ('reg query "hkcu\control panel\international" /v sshortdate') do set sfmt=%%a
for /f "tokens=3,3" %%a in ('reg query "hkcu\control panel\international" /v slongdate') do set lfmt=%%a

reg add "hkcu\control panel\international" /v sshortdate /t reg_sz /d yyyy-MM-dd /f >nul
reg add "hkcu\control panel\international" /v slongdate /t reg_sz /d yyyy-MM-dd /f >nul

set ymd=%date%

reg add "hkcu\control panel\international" /v sshortdate /t reg_sz /d %sfmt% /f >nul
reg add "hkcu\control panel\international" /v slongdate /t reg_sz /d %lfmt% /f >nul
rem ----------------------------------------------
call :lower repol %repo%
rem ----------------------------------------------
echo %app%
echo %description%
echo.
echo Version: v%version%
echo Date:    %ymd%
echo.
echo Requires SDK .NET 8.0 to build
echo Requires .NET Desktop Runtime 8.0 to run
echo Download from https://dotnet.microsoft.com/download
echo.
echo Run once to create %app%.config.json
echo and correct it
echo.
echo https://github.com/diev/%repo%
echo https://gitverse.ru/diev/%repo%
echo https://gitflic.ru/project/diev/%repol%
rem ----------------------------------------------
goto :eof
