::-------------------------------------------::
::           buildReshiramCCMod.bat          ::
::-------------------------------------------::
::                 Arguments                 ::
::-------------------------------------------::
:: %0 - This Script                          ::
:: %1 - Build Configuration (debug, release) ::
::-------------------------------------------::

@echo off

:: If there is no build configuration provided, default to debug
if not "%~1"=="" (
    set buildConfig=Debug
)
else (
    set buildConfig=%~1
)

:: Clean the project so that there are no residual files
dotnet clean %CD%\ReshiramCCMod.csproj

:: Build the project
dotnet build %CD%\ReshiramCCMod.csproj /property:GenerateFullPaths=true /p:Configuration=%buildConfig% /p:Platform="AnyCPU" /consoleloggerparameters:NoSummary

:: Run the project (if it successfully built)
dotnet run

pause