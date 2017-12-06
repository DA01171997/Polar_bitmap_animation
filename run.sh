#!/bin/bash

#File: run.sh
#Author: Duy Do
#Author's email: duy.ado@fullerton.edu
#Author's Github: https://github.com/DA01171997

echo "***Remove old binary files.***"
rm *.dll
rm *.exe

echo "***View the list of source files.***"
ls -l

echo "***Compile TESTframe.cs to create the file: TESTframe.dll.***"
mcs -target:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:TESTframe.dll TESTframe.cs

echo "***Compile TESTmain.cs and link previously created dll file to create an executable file.***"
mcs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:TESTframe.dll -out:TEST.exe TESTmain.cs

echo "***View the list of files in the current folder.***"
ls -l

echo "***Run TEST program.***"
./TEST.exe

echo "***Remove dll files.***"
rm *.dll

echo "***The script has terminated.***"
