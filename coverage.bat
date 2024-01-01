@echo off

setlocal

call ..\Certification\set_tokens.bat

set PROJECTNAME=Method.Contracts
set TOKEN=%METHOD_CONTRACTS_CODECOV_TOKEN%
set TESTPROJECTNAME=%PROJECTNAME%.Test
set RESULTFILENAME=Coverage-%PROJECTNAME%.xml
set OPENCOVER_VERSION=4.7.1221
set OPENCOVER=OpenCover.%OPENCOVER_VERSION%
set CODECOV_VERSION=1.12.2
set CODECOV=Codecov.%CODECOV_VERSION%
set NUINT_CONSOLE_VERSION=3.11.1
set NUINT_CONSOLE=NUnit.ConsoleRunner.%NUINT_CONSOLE_VERSION%
set FRAMEWORK=net481

nuget install OpenCover -Version %OPENCOVER_VERSION% -OutputDirectory packages
nuget install CodeCov -Version %CODECOV_VERSION% -OutputDirectory packages
nuget install NUnit.ConsoleRunner -Version %NUINT_CONSOLE_VERSION% -OutputDirectory packages

if not exist ".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" goto error_console1
if not exist ".\packages\%CODECOV%\tools\codecov.exe" goto error_console2
if not exist ".\packages\%NUINT_CONSOLE%\tools\nunit3-console.exe" goto error_console3

if exist ".\Test\%TESTPROJECTNAME%\publish" rd /S /Q ".\Test\%TESTPROJECTNAME%\publish"

dotnet build ./Test/%TESTPROJECTNAME% -c Debug -f %FRAMEWORK% /p:Platform=x64

if exist .\Test\%TESTPROJECTNAME%\*.log del .\Test\%TESTPROJECTNAME%\*.log
if exist .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME% del .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%
".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" -register:user -target:".\packages\%NUINT_CONSOLE%\tools\nunit3-console.exe" -targetargs:".\Test\%TESTPROJECTNAME%\bin\x64\Debug\%FRAMEWORK%\%TESTPROJECTNAME%.dll --trace=Debug --labels=Before" "-filter:+[*]* -[%TESTPROJECTNAME%*]*" -output:".\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%"
if exist .\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME% .\packages\%CODECOV%\tools\codecov -f ".\Test\%TESTPROJECTNAME%\obj\x64\Debug\%RESULTFILENAME%" -t %TOKEN%
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
