//File: RicochetBallmain.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: RicochetBall
//Purpose: Creating a UI that similar to billiard table. User get to catches the ball for point.
//Date last modified: 12/04/2017
//Source Files in this program: RicochetBallframeframe.cs, RicochetBallframetmain.cs, run.sh
//Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal.  

using System;
using System.Drawing;
using System.Windows.Forms;
public class RicochetBallmain {
	public static void Main() {
	System.Console.WriteLine("RicochetBallframe program starts.");
	RicochetBallframe T = new RicochetBallframe();
	Application.Run(T);
	System.Console.WriteLine("RicochetBallframe program has ended");
	}
}