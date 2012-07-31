call "%VS100COMNTOOLS%\..\..\VC\vcvarsall.bat" x86
if not exist nuget.exe (call download-nuget)
set ARGS=
if not [%1]==[] set ARGS=/p:NuSpec=%1
msbuild nuget.proj /t:BuildPackage %ARGS%
