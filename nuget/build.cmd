@echo off
call ..\common.bat

set ARGS=
if not [%1]==[] set ARGS=/p:NuSpec=%1
msbuild nuget.proj /t:BuildPackage %ARGS%
