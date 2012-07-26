#!/bin/sh

CONF=$1
PATH=./$CONF:$PWD/Release:$PATH

for dll in `ls $CONF/*.Test.dll`
do
    ./nunit/nunit-console-x86.exe -domain=None $dll
done
