@echo off
call common.bat

rem You can override build properties with arguments, e.g.: /p:Configuration=Release
msbuild CefSharp.proj %*
