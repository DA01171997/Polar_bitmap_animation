#!/bin/bash

#File: run.sh
#Author: Duy Do
#Author's email: duy.ado@fullerton.edu
#Author's Github: https://github.com/DA01171997
#Project: RicochetBall
#Purpose: Creating a UI that similar to billiard table. User get to catches the ball for point.
#Date last modified: 12/04/2017
#Source Files in this program: RicochetBallframeframe.cs, RicochetBallframetmain.cs, run.sh
#Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal.  


echo "***Remove old binary files.***"
rm *.dll
rm *.exe

echo "***View the list of source files.***"
ls -l

echo "***Compile RicochetBallframe.cs to create the file: TRicochetBallframe.dll.***"
mcs -target:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:RicochetBallframe.dll RicochetBallframe.cs

echo "***Compile RicochetBallmain.cs and link previously created dll file to create an executable file.***"
mcs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:RicochetBallframe.dll -out:RicochetBall.exe RicochetBallmain.cs

echo "***View the list of files in the current folder.***"
ls -l

echo "***Run TEST program.***"
./RicochetBall.exe

echo "***Remove dll files.***"
rm *.dll

echo "***The script has terminated.***"
