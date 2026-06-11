@echo off

setlocal

nuget install dlebansais.Packager -Version 2.2.1 -OutputDirectory packages

.\packages\dlebansais.Packager.2.2.1\lib\net10.0-windows7.0\Packager.exe
echo ERRORLEVEL: %ERRORLEVEL%