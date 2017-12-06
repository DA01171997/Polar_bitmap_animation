//File: TrafficLightfame.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: Traffic Light 
//Purpose: Creating a UI Traffic Light with 3 different switching speeds using a timer. *Default Speed = SLOW*
//Date last Modified: 10/17/2017 
//Source Files in this program: TrafficLightframe.cs, TrafficLightmain.cs, run.sh
//Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal. 
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class TESTframe : Form {
	private const int xframe = 1280; //UI X's size
	private const int yframe = 800;	//UI Y's size
	private const int pensize = 1;
	private Button exitB = new System.Windows.Forms.Button();
	private Point exitBL = new Point(530,500);	
	private Size buttonSize = new Size(60,50);

	public TESTframe () {
                // Initialize the UI
		Text = "TEST by Duy Do";
                System.Console.WriteLine("xframe={0}. yframe={1}.", xframe, yframe);
                Size = new Size(xframe,yframe);
                BackColor = Color.White;
		// Initialize Buttons
	
		exitB.Text = "Exit";
		exitB.Size = buttonSize;
		exitB.Location = exitBL;
		exitB.BackColor = Color.Yellow;
		exitB.TabIndex = 2;
	
		
		//Add Buttons to frame
		Controls.Add(exitB);
		
		// Add live handlers to UI
		exitB.Click += new EventHandler(exitBP);
	}



	protected void exitBP(Object sender, EventArgs events) {
	System.Console.WriteLine("Exit");
	Close(); //exit
	}
	
	protected override void OnPaint(PaintEventArgs light) {
		Graphics DuyLight = light.Graphics;
		
	}
	
}