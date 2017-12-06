//File: TravelBallmain.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: TravelBall
//Purpose: Creating a UI that animal ball moving linear motion with user degree and c# input.
//Date last modified: 12/02/2017
//Source Files in this program: TravelBallframe.cs, TravelBalltmain.cs, run.sh
//Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal. 

using System;
using System.Drawing;
using System.Windows.Forms;
public class TravelBallmain {
	public static void Main() {
	System.Console.WriteLine("TravelBall program starts.");
	TravelBallframe T = new TravelBallframe();
	Application.Run(T);
	System.Console.WriteLine("TravelBall program has ended");
	}
}