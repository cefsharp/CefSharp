#!/bin/sh

if [ -z $1 ]; then
  CEF="/c/src/chromium/src/cef"
else
  CEF=$1
fi

cp -r $CEF/include .
git clean -df include

for CONFIG in Release Debug
do
  cp $CEF/$CONFIG/lib/libcef.lib libs/$CONFIG/
  cp $CEF/$CONFIG/lib/libcef_dll_wrapper.lib libs/$CONFIG/

  mkdir -p $CONFIG/
  cp $CEF/$CONFIG/*.dll $CONFIG/
  cp $CEF/$CONFIG/chrome.pak $CONFIG/
  cp -r $CEF/$CONFIG/locales $CONFIG/
done
