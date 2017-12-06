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

echo "***Compile CurvesAlgorithm.cs to create the file: CurvesAlgorithm..dll.***"
mcs -target:library CurvesAlgorithm.cs -out:CurvesAlgorithm.dll

echo "***Compile Curvesframe.cs to create the file: Curvesframe.dll.***"
mcs -target:library Curvesframe.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:CurvesAlgorithm.dll -out:Curvesframe.dll

echo "***Compile Curvesmain.cs and link previously created dll file to create an executable file.***"
mcs -target:exe Curvesmain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Curvesframe.dll -out:Curves.exe

echo "***View the list of files in the current folder.***"
ls -l

echo "***Run Curves program.***"
./Curves.exe

echo "***Remove dll files.***"
rm *.dll

echo "***The script has terminated.***"
