#!/bin/sh

CONF=$1

for dll in `ls $CONF/*.Test.dll`
do
    ./nunit/nunit-console-x86.exe -domain=None $dll
done
