//File: TravelBallfame.cs
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
using System.Timers;

public class TravelBallframe : Form {
	private const int xframe = 800; //UI X's size
	private const int yframe = 1000;//UI Y's size
	private const int pensize = 1;
	private const int cRadius =10;
	private const double refresh_rate = 30.0;//Hertz: How many times per second the display area is repainted.
	private double ball_refresh_rate=60.0;
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
	private Point speedBL = new Point(50,850);
	private Point degreeBL = new Point(110,850);
	private Size buttonSize = new Size(60,50);
	private TextBox speedInput = new TextBox();
	private TextBox degreeInput = new TextBox();
	private float speedD = 0;
	private float degreeD = 0;
	private float mRadian = 0;
	private float mathToCRatio=50;
	private float xMath=0;
	private float yMath=0;
	private float cxMath;
	private float cyMath;
	private float cSpeedTic;
	private float mSpeedTic;
	private bool startP=false;
	private String cCordMessage = "C# Coordinates: ";
	private String MathCordMessage = "Math Coordinates: ";
	private String myName = "Duy Do";
	//Declare clocks
	private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer motion_ball_clock = new System.Timers.Timer();
	
	public TravelBallframe () {
        // Initialize the UI
		Text = "TEST by Duy Do";
        System.Console.WriteLine("xframe={0}. yframe={1}.", xframe, yframe);
        Size = new Size(xframe,yframe);
        //BackColor = Color.White;
		BackColor = Color.DeepPink;
		// Initialize Buttons
		ballpointpen = new Pen(Color.Black, pensize);
		ballpointpenDash = new Pen(Color.Black, pensize);
		ballpointpenDash.DashPattern = new float[] {2.0f,6.0f};
		
		// Initialize Buttons
		exitB.Text = "Exit";
		exitB.Size = buttonSize;
		exitB.Location = exitBL;
		exitB.BackColor = Color.Salmon;
		exitB.TabIndex = 3;
		
		pauseB.Text = "Pause";
		pauseB.Size = buttonSize;
		pauseB.Location = pauseBL;
		pauseB.BackColor = Color.Salmon;
		pauseB.TabIndex = 4;
		
		startB.Text = "Start";
		startB.Size = buttonSize;
		startB.Location = startBL;
		startB.BackColor = Color.Salmon;
		startB.TabIndex = 5;
	
		// Initialize Text box
		speedInput.Size=buttonSize;
		speedInput.Location= speedBL;
		speedInput.Font = new Font("Arial", 10, FontStyle.Regular);
		speedInput.Text = "Speed";
		speedInput.BackColor = Color.Salmon;
		speedInput.TabIndex =1;
		
		degreeInput.Size=buttonSize;
		degreeInput.Location= degreeBL;
		degreeInput.Font = new Font("Arial", 10, FontStyle.Regular);
		degreeInput.Text = "Degree";
		degreeInput.BackColor = Color.Salmon;
		degreeInput.TabIndex =2;
		
		// Prepare the refresh clock
		graphic_area_refresh_clock.Enabled = false;
		graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_graphic_area);
		// Prepare the refresh clock
		motion_ball_clock.Enabled = false;
		motion_ball_clock.Elapsed += new ElapsedEventHandler(Update_the_ball);
		
		// Add Buttons to frame
		Controls.Add(exitB);
		Controls.Add(pauseB);
		Controls.Add(speedInput);
		Controls.Add(degreeInput);
		Controls.Add(startB);

		// Add live handlers to UI
		exitB.Click += new EventHandler(exitBP);
		pauseB.Click += new EventHandler(pauseBP);
		startB.Click += new EventHandler(startBP);
		
	}
	protected void exitBP(Object sender, EventArgs events) {
	System.Console.WriteLine("Exit");
	Close(); //exit
	}
	protected void pauseBP(Object sender, EventArgs events) {
	
	if (motion_ball_clock.Enabled==true && graphic_area_refresh_clock.Enabled==true&&startP){	//Check if clock is paused
		motion_ball_clock.Stop();
		graphic_area_refresh_clock.Stop();
		motion_ball_clock.Stop();
		pauseB.Text = "Resume";
		startP=false;
		System.Console.WriteLine("Paused");
	}
	else if (motion_ball_clock.Enabled==false && graphic_area_refresh_clock.Enabled==false&&!startP) { //check if clock is started
		motion_ball_clock.Start();
		graphic_area_refresh_clock.Start();
		motion_ball_clock.Start();
		pauseB.Text = "Pause";
		startP=true;
		System.Console.WriteLine("Resumed");
	}
	Invalidate();
	}
	protected void startBP(Object sender, EventArgs events) {
	System.Console.WriteLine("Start");
	startP=true;
	pauseB.Text = "Pause";
	//Start both clocks running
	if (speedInput.Text != "Speed"&&degreeInput.Text != "Degree"){
	Start_graphic_clock(refresh_rate);
	Start_motion_ball_clock(ball_refresh_rate);
	}
	Invalidate();
	}
   protected void Start_graphic_clock(double refreshrate)
    {double elapsedtimebetweentics;
    if(refreshrate < 1.0) refreshrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = time_converter/refreshrate;  //elapsed time between tics has units milliseconds
    graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    graphic_area_refresh_clock.Enabled = true;  //Start this clock ticking
    System.Console.WriteLine("Method Start_graphic_clock has terminated.");
   }
   protected void Start_motion_ball_clock(double ballrefreshrate) {
	   double elapsedtimebetweentics;
    if(ballrefreshrate < 1.0) ballrefreshrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = time_converter/ballrefreshrate;  //elapsed time between tics has units milliseconds
    motion_ball_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    motion_ball_clock.Enabled = true;  //Start this clock ticking
    System.Console.WriteLine("Method Start_motion_ball_clock has terminated.");
   }
   protected void Update_the_graphic_area(System.Object sender, ElapsedEventArgs even)
    {   System.Console.WriteLine("refreshing screen");
		Invalidate();
   }
   protected void Update_the_ball(System.Object sender, ElapsedEventArgs even){
	    if(startP&&speedInput.Text != "Speed"&&degreeInput.Text != "Degree"){	//Check for Degree and Speed is provided
			if (cxMath>=0&&cxMath<=xframe&&cyMath>=0&&cyMath<=xframe) { 		//Check to stop motion out of bound
				speedD=float.Parse(speedInput.Text);							
				degreeD=float.Parse(degreeInput.Text);
				cSpeedTic=speedD/(float)ball_refresh_rate;
				mSpeedTic=cSpeedTic/mathToCRatio;
				mRadian=DegreeToRadian(degreeD);
				xMath+=xMathNext(mSpeedTic, mRadian);							//Compute next X
				yMath+=yMathNext(mSpeedTic, mRadian);							//Compute next Y
			}
			else {
				graphic_area_refresh_clock.Stop();		
				motion_ball_clock.Stop();
			}
		}
		Invalidate();
   }
   protected float DegreeToRadian(float degree) {								//Function converts degree to radian
	   float pI=(float)Math.PI;
	   return (degree*pI)/180;
   }
   protected float xMathNext(float mspeedtic, float radians){					//Function compute delta x
	   return (mspeedtic*((float)Math.Cos((double) radians)));
   }
   protected float yMathNext(float mspeedtic, float radians){					//Function compute delta y
	   return (mspeedtic*((float)Math.Sin((double) radians)));
   }
   
    
	protected override void OnPaint(PaintEventArgs travelBall) {
		Graphics Duyball = travelBall.Graphics;
		cxMath= (xMath * mathToCRatio + (xframe/2));							// Convert math x to C# x
		cyMath= ((xframe/2) - yMath * mathToCRatio );							// Convert math y to C# y
		
		//Draw interface box
		Duyball.FillRectangle(Brushes.Peru,0, xframe, xframe-1, yframe-1);											
		Duyball.DrawString(myName,fontStyle, drawBrush, 50,50);
		Duyball.DrawString("Coordinates of center of ball:", fontStyle, drawBrush, 275,810);
		Duyball.DrawString(cCordMessage+"(" + cxMath + "," + cyMath + ")", fontStyle, drawBrush, 275, 850);
		Duyball.DrawString(MathCordMessage+"(" + xMath + "," + yMath + ")", fontStyle, drawBrush, 275,890);
		
		//Draw Elipse and XY Coordinates
		Duyball.FillEllipse(Brushes.DarkTurquoise,(cxMath-cRadius),(cyMath -cRadius),(2* cRadius),(2 *cRadius));
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
		}
	}
	
}