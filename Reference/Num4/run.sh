#!/bin/bash

#File: run.sh
#Author: Duy Do
#Author's email: duy.ado@fullerton.edu
#Author's Github: https://github.com/DA01171997
#Purpose: Creating a UI that animal ball moving linear motion with user degree and c# input.
#Date last modified: 12/02/2017
#Source Files in this program: TravelBallframe.cs, TravelBalltmain.cs, run.sh
#Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal.

echo "***Remove old binary files.***"
rm *.dll
rm *.exe

echo "***View the list of source files.***"
ls -l

echo "***Compile TravelBallframe.cs to create the file: TravelBallframe.dll.***"
mcs -target:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:TravelBallframe.dll TravelBallframe.cs

echo "***Compile TravelBallmain.cs and link previously created dll file to create an executable file.***"
mcs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:TravelBallframe.dll -out:TravelBall.exe TravelBallmain.cs

echo "***View the list of files in the current folder.***"
ls -l

echo "***Run TEST program.***"
./TravelBall.exe

echo "***Remove dll files.***"
rm *.dll

echo "***The script has terminated.***"
