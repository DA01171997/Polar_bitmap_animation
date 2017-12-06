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
private const int xframe = 800; //UI X's size
	private const int yframe = 1000;//UI Y's size
	private const int pensize = 1;
	private const int cRadius =10;
	private int pointCounter =0;
	private int destroyCounter=0;
	private const double refresh_rate = 30.0;//Hertz: How many times per second the display area is repainted.
	private const double ball_refresh_rate=50.0;
	private const double restart_clock_rate=1.0;
	private const double destroy_clock_rate=50.0;
	private const double time_converter=1000.0; //Timer Interval is in milisecond. This is 1 second
	private Pen ballpointpen;
	private Pen ballpointpenDash;
	private Font fontStyle = new Font("Arial", 10);
	private SolidBrush drawBrush = new SolidBrush(Color.Black);
	private Button startB = new System.Windows.Forms.Button();
	private Button exitB = new System.Windows.Forms.Button();
	private Button pauseB = new System.Windows.Forms.Button();
	private Point startBL = new Point(530,850);	
	private Point exitBL = new Point(650,850);	
	private Point pauseBL = new Point(590,850);	
	private Size buttonSize = new Size(60,50);
	private SolidBrush elipseBrush= new SolidBrush(Color.DarkTurquoise);
	private float speedD = 100;		//initial speed 40pix sec
	private float degreeD = 0;
	private float mRadian = 0;
	private float mathToCRatio=50;
	private float xMath=0;
	private float yMath=0;
	private float cxMath;
	private float cyMath;
	private float cSpeedTic;
	private float mSpeedTic;
	private bool destroyed=false;
	private bool caught=false;
	private float cursor_x = 0; //coordinate of mouse
	private float cursor_y = 0; //coordinate of mouse
	private String cCordMessage = "Ball's location = ";
	private String MathCordMessage = "Points earned = ";
	private String ballspeedmessage = "Ball's speed = ";
	private String degreemessage = "Random Degree = ";
	private String myName = "Duy Do";
	private Random rnddegree = new Random();
	private int minRandTheta = -360;
	private int maxRandTheta = 360;

	public TESTframe () {
                // Initialize the UI
		Text = "TEST by Duy Do";
                System.Console.WriteLine("xframe={0}. yframe={1}.", xframe, yframe);
                Size = new Size(xframe,yframe);
                BackColor = Color.DeepPink;
		ballpointpen = new Pen(Color.Black, pensize);
		ballpointpenDash = new Pen(Color.Black, pensize);
		ballpointpenDash.DashPattern = new float[] {2.0f,6.0f};
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
	  protected float xMathTocX( float x) {										//Function that convert math X to C# X
	   return(x * mathToCRatio + (xframe/2));
	   
   }
   protected float yMathTocY( float y) {										//Function that convert math Y to C# Y
	   return((xframe/2) - y * mathToCRatio );
	   
   }
	
	protected override void OnPaint(PaintEventArgs bitMap) {
		Graphics Duyball = bitMap.Graphics;							
		cxMath = xMathTocX(xMath);													// Convert math x to C# x
		cyMath = yMathTocY(yMath);													// Convert math y to C# y

		//Draw interface box
		Duyball.FillRectangle(Brushes.Peru,0, xframe, xframe-1, yframe-1);											
		Duyball.DrawString(myName,fontStyle, drawBrush, 50,50);
		Duyball.DrawString(ballspeedmessage + speedD + " pix/sec", fontStyle, drawBrush, 275,810);
		Duyball.DrawString(cCordMessage+"(" + cxMath + "," + cyMath + ")", fontStyle, drawBrush, 275, 850);
		Duyball.DrawString(MathCordMessage+ pointCounter, fontStyle, drawBrush, 275,890);
		Duyball.DrawString(degreemessage + degreeD, fontStyle, drawBrush, 275,930);
		
		//Draw Elipse and XY Coordinates
		// if (!destroyed){
		// Duyball.FillEllipse(elipseBrush,(cxMath-cRadius),(cyMath -cRadius),(2* cRadius),(2 *cRadius));
		// }
		Duyball.DrawLine(ballpointpenDash, 0, xframe/2, xframe/2, xframe/2);
		Duyball.DrawLine(ballpointpenDash, xframe/2,xframe/2, xframe/2, xframe);
		Duyball.DrawLine(ballpointpen, xframe/2, 0, xframe/2, xframe/2);
		Duyball.DrawLine(ballpointpen, xframe/2,xframe/2, xframe, xframe/2);
		
		//Initialize XY Coordinates
		for (int i=1; i<=xframe/2/mathToCRatio; i++){
		string temp = i.ToString();
		//Draw Numbers
		Duyball.DrawString(temp, fontStyle, drawBrush, xframe/2 + i*mathToCRatio, xframe/2 + 10);
		Duyball.DrawString(temp, fontStyle, drawBrush, xframe/2 + 10, xframe/2 - i*mathToCRatio);
		Duyball.DrawString("-"+temp, fontStyle, drawBrush, xframe/2 - i*mathToCRatio, xframe/2 + 10);
		Duyball.DrawString("-"+temp, fontStyle, drawBrush,xframe/2 + 10, xframe/2 + i*mathToCRatio );
		Duyball.DrawLine(ballpointpen, xframe/2-10, xframe/2 - i*mathToCRatio, xframe/2+10, xframe/2-i*mathToCRatio);
		//Draw Number Dashes
		Duyball.DrawLine(ballpointpen, xframe/2+i*mathToCRatio, xframe/2-10, xframe/2+i*mathToCRatio, xframe/2+10);
		Duyball.DrawLine(ballpointpen, xframe/2-10, xframe/2 - i*mathToCRatio, xframe/2+10, xframe/2-i*mathToCRatio);
		Duyball.DrawLine(ballpointpen, 0 + i*mathToCRatio, xframe/2 - 10, 0 + i*mathToCRatio, xframe/2 + 10);
		Duyball.DrawLine(ballpointpen, xframe/2-10, xframe/2 + i*mathToCRatio, xframe/2+10, xframe/2+i*mathToCRatio);
		
		Duyball.DrawEllipse(ballpointpen, (400-i*mathToCRatio),(400-i*mathToCRatio),(2*i*mathToCRatio),(2*i*mathToCRatio));
		
		}
	}
	
}