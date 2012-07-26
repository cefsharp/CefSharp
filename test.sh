#!/bin/sh

CONF=$1
PATH=./$CONF:$PWD/Release:$PATH

./nunit/nunit-console-x86.exe -domain=None -process=Multiple $CONF/*.Test.dll
