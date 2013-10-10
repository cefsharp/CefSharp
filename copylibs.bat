@echo off
call common.bat

msbuild CefSharp.targets /t:CopyCefLibs /p:Configuration=Debug
msbuild CefSharp.targets /t:CopyCefLibs /p:Configuration=Release
