#!/bin/sh
#
# Purpose: Utility script which makes it easier to switch between VS2010 and VS2012 (since they share the same output folder)
#
rm -rf $(gfind -iname bin -or -iname obj)
rm -f Debug/CefSharp*.*
rm -f Release/CefSharp*.*
