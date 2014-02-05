#!/bin/sh

zip -9r CefSharp.zip * -x Debug/\* -x \*/obj/* -x \*/bin/* -x *.DS_Store -x *.*sdf -x *.suo -x *.zip -x *.nupkg -x *.diff -x ipch/\* -x _ReSharper\*/*
