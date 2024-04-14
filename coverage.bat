@echo off

setlocal

call ..\Certification\set_tokens.bat

set PROJECTNAME=Method.Contracts
set TOKEN=%METHOD_CONTRACTS_CODECOV_TOKEN%
set TESTPROJECTNAME=%PROJECTNAME%.Test
set RESULTFILENAME=Coverage-%PROJECTNAME%.xml
set OPENCOVER_VERSION=4.7.1221
set OPENCOVER=OpenCover.%OPENCOVER_VERSION%
set CODECOV_UPLOADER_VERSION=0.7.2
set CODECOV_UPLOADER=CodecovUploader.%CODECOV_UPLOADER_VERSION%
set NUINT_CONSOLE_VERSION=3.15.5
set NUINT_CONSOLE=NUnit.ConsoleRunner.%NUINT_CONSOLE_VERSION%
set FRAMEWORK=net481

nuget install OpenCover -Version %OPENCOVER_VERSION% -OutputDirectory packages
nuget install CodecovUploader -Version %CODECOV_UPLOADER_VERSION% -OutputDirectory packages
nuget install NUnit.ConsoleRunner -Version %NUINT_CONSOLE_VERSION% -OutputDirectory packages

if not exist ".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" goto error_console1
if not exist ".\packages\%CODECOV_UPLOADER%\tools\codecov.exe" goto error_console2
if not exist ".\packages\%NUINT_CONSOLE%\tools\nunit3-console.exe" goto error_console3

if exist ".\Test\%TESTPROJECTNAME%\publish" rd /S /Q ".\Test\%TESTPROJECTNAME%\publish"

dotnet build ./Test/%TESTPROJECTNAME% -c Debug -f %FRAMEWORK% /p:Platform=x64

if exist .\Test\%TESTPROJECTNAME%\*.log del .\Test\%TESTPROJECTNAME%\*.log
if exist .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME% del .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%
".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" -register:user -target:".\packages\%NUINT_CONSOLE%\tools\nunit3-console.exe" -targetargs:".\Test\%TESTPROJECTNAME%\bin\x64\Debug\%FRAMEWORK%\%TESTPROJECTNAME%.dll --trace=Debug --labels=Before" "-filter:+[*]* -[%TESTPROJECTNAME%*]*" -output:".\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%"
if exist .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME% .\packages\%CODECOV_UPLOADER%\tools\codecov -f ".\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%" -t %TOKEN%
goto end

:error_console1
echo ERROR: OpenCover.Console not found.
goto end

:error_console2
echo ERROR: Codecov not found.
goto end

:error_console3
echo ERROR: nunit3-console not found.
goto end

:error_not_built
echo ERROR: %TESTPROJECTNAME%.dll not built (both Debug and Release are required).
goto end

:end
del *.log
if exist TestResult.xml del TestResult.xml
