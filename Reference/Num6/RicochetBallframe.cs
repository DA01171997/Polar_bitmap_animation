//File: RicochetBallframe.cs
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
using System.Timers;

public class RicochetBallframe : Form {
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
	//Declare clocks
	private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer motion_ball_clock = new System.Timers.Timer();
	private static System.Timers.Timer restart_clock = new System.Timers.Timer();
	private static System.Timers.Timer destroy_clock = new System.Timers.Timer();
	//destroy animation variable
	private float xMath2=0;
	private float yMath2=0;
	private float cxMath2;
	private float cyMath2;
	private float xMath3=0;
	private float yMath3=0;
	private float cxMath3;
	private float cyMath3;
	private float xMath4=0;
	private float yMath4=0;
	private float cxMath4;
	private float cyMath4;
	private float xMath5=0;
	private float yMath5=0;
	private float cxMath5;
	private float cyMath5;
	
	public RicochetBallframe () {
        // Initialize the UI
		Text = "RicochetBall by Duy Do";
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
		
		pauseB.Text = "Next ball";
		pauseB.Size = buttonSize;
		pauseB.Location = pauseBL;
		pauseB.BackColor = Color.Salmon;
		pauseB.TabIndex = 4;
		
		startB.Text = "New Game";
		startB.Size = buttonSize;
		startB.Location = startBL;
		startB.BackColor = Color.Salmon;
		startB.TabIndex = 5;
	
		// Prepare the refresh clock
		graphic_area_refresh_clock.Enabled = false;
		graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_graphic_area);
		// Prepare the motion clock
		motion_ball_clock.Enabled = false;
		motion_ball_clock.Elapsed += new ElapsedEventHandler(Update_the_ball);
		// Prepare the motion clock
		restart_clock.Enabled = false;
		restart_clock.Elapsed += new ElapsedEventHandler(Restart_clocks);
		// Prepare the destroy animation motion clock
		destroy_clock.Enabled = false;
		destroy_clock.Elapsed += new ElapsedEventHandler(Destroys_clocks);
		// Add Buttons to frame
		Controls.Add(exitB);
		Controls.Add(pauseB);
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
	System.Console.WriteLine("Next Ball");
	resetPosition();
	Start_restart_clock(restart_clock_rate);
	Invalidate();
	}
	protected void startBP(Object sender, EventArgs events) {
	System.Console.WriteLine("New Game");
	speedD=40;
	resetPosition();
	pointCounter=0;
	Start_graphic_clock(refresh_rate);
	Start_motion_ball_clock(ball_refresh_rate);
	Invalidate();
	}
	
	//Starting clocks methods 
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
     protected void Start_destroy_clock(double destroy_clockrate) {
	   double elapsedtimebetweentics;
    if(destroy_clockrate < 1.0) destroy_clockrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = time_converter/destroy_clockrate;  //elapsed time between tics has units milliseconds
    destroy_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    destroy_clock.Enabled = true;  //Start this clock ticking
    System.Console.WriteLine("Method Start_destroy_clock has terminated.");
   }
    protected void Start_restart_clock(double restartrefreshrate) {
	   double elapsedtimebetweentics;
    if(restartrefreshrate < 1.0) restartrefreshrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = time_converter/restartrefreshrate;  //elapsed time between tics has units milliseconds
    restart_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    restart_clock.Enabled = true;  //Start this clock ticking
    System.Console.WriteLine("Method Start_restart_clock has terminated.");
   }
   protected void Update_the_graphic_area(System.Object sender, ElapsedEventArgs even)
    {   System.Console.WriteLine("Refreshing Screen");
		Invalidate();
   }
   
   
   
   
   // Clock Methods
   protected void Update_the_ball(System.Object sender, ElapsedEventArgs even){ // Update ball motion clock
	   	cSpeedTic=speedD/(float)ball_refresh_rate;
		mSpeedTic=cSpeedTic/mathToCRatio;
		if(!caught){															//Check if ball is caught
			if(cyMath<=0||cyMath+2*cRadius>=xframe){
				degreeD= -1*degreeD;
				System.Console.WriteLine("BOUNCE");
			}
			else if(cxMath<=0||cxMath+2*cRadius>=xframe) {
			degreeD = -1*degreeD -180;
			}
			mRadian=DegreeToRadian(degreeD);
			xMath+=xMathNext(mSpeedTic, mRadian);							//Compute next X
			yMath+=yMathNext(mSpeedTic, mRadian);							//Compute next Y
		}
		else if(caught){
			System.Console.WriteLine("Starting Destroy Animation");
			Start_destroy_clock(destroy_clock_rate);
			motion_ball_clock.Stop();
			motion_ball_clock.Enabled=false;
			graphic_area_refresh_clock.Stop();
			graphic_area_refresh_clock.Enabled=false;
		}
		Invalidate();
   }

    protected void Destroys_clocks(System.Object sender, ElapsedEventArgs even){	//Destroy animation clock
		if(destroyCounter<=((int)destroy_clock_rate/2)){							//Set exploding tiny ball at caught location
			xMath2=xMath;							
			yMath2=yMath;
			
			xMath3=xMath;
			yMath3=yMath;
			
			xMath4=xMath;							
			yMath4=yMath;
			
			xMath5=xMath;							
			yMath5=yMath;
		}
		
		else if(destroyCounter<=((int)destroy_clock_rate/2)&&destroyCounter%2!=0){				// exploading color
		elipseBrush.Color= Color.Red;
		}
		else if(destroyCounter<=((int)destroy_clock_rate/2)&&destroyCounter%2==0){				// exploading color
		elipseBrush.Color= Color.DarkTurquoise;
		}
		else if (destroyCounter>=((int)destroy_clock_rate/2)&&destroyCounter<destroy_clock_rate){	//send exploaded ball away
			destroyed=true;
			xMath2+=xMathNext(mSpeedTic, ((float)Math.PI/4));							
			yMath2+=yMathNext(mSpeedTic, ((float)Math.PI/4));
			
			xMath3+=xMathNext(mSpeedTic, (float)(3*Math.PI/4));							
			yMath3+=yMathNext(mSpeedTic, (float)(3*Math.PI/4));
			
			xMath4+=xMathNext(mSpeedTic,(float)(5*Math.PI/4));							
			yMath4+=yMathNext(mSpeedTic,(float)(5*Math.PI/4));
			
			xMath5+=xMathNext(mSpeedTic, (float)(7*Math.PI/4));							
			yMath5+=yMathNext(mSpeedTic, (float)(7*Math.PI/4));
		}
		else if (destroyCounter==destroy_clock_rate) {                                          // Start restarting clock after exploading animation
			System.Console.WriteLine("Destroyed! Starting Restart clock");
			destroyCounter=0;
			destroyed=false;
			//Start_restart_clock(restart_clock_rate);
			resetPosition();
			destroy_clock.Stop();
			destroy_clock.Enabled=false;
		}
		destroyCounter++;
		System.Console.WriteLine(destroyCounter);
		Invalidate();
   }
   protected void Restart_clocks(System.Object sender, ElapsedEventArgs even){					// Wait 1 second to reset the ball to center
	   destroy_clock.Stop();
	   destroy_clock.Enabled=false;
	   restart_clock.Stop();
	   restart_clock.Enabled=false;
	   speedD=40*(float)Math.Pow(1.20,pointCounter);
	   resetPosition();
	   Start_graphic_clock(refresh_rate);
	   Start_motion_ball_clock(ball_refresh_rate);
	   System.Console.WriteLine("Restarted new speed "+speedD);
	   
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
      protected override void OnMouseDown(MouseEventArgs me)					//Function to check caught ball
   {  cursor_x = (float)me.X;
      cursor_y = (float)me.Y; 
	  if (cursor_x >= cxMath&&cursor_x <= cxMath+2*cRadius || cursor_y >= cyMath&&cursor_y <= cyMath +2*cRadius) {
		pointCounter++;
		System.Console.WriteLine("Ball Caught");
		caught=true;
	  }	   
      Invalidate();
   }
   protected float randThetaGenerate(int minRandTheta, int maxRandTheta) {		//Random degree generator 
		return (float)rnddegree.Next(minRandTheta, maxRandTheta); 
   }
    protected void resetPosition(){												//Reset everything method
	System.Console.WriteLine("Reset");
	xMath=0;
	yMath=0;
	destroyed=false;
	caught=false;
	degreeD=randThetaGenerate(minRandTheta, maxRandTheta);
   }
   protected float xMathTocX( float x) {										//Function that convert math X to C# X
	   return(x * mathToCRatio + (xframe/2));
	   
   }
   protected float yMathTocY( float y) {										//Function that convert math Y to C# Y
	   return((xframe/2) - y * mathToCRatio );
	   
   }
	protected override void OnPaint(PaintEventArgs travelBall) {
		Graphics Duyball = travelBall.Graphics;							
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
		if (!destroyed){
		Duyball.FillEllipse(elipseBrush,(cxMath-cRadius),(cyMath -cRadius),(2* cRadius),(2 *cRadius));
		}
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
		//Draw Exploding circle
		if (destroyed){
		// Convert math coordinates to c # coordinate and draw exploading circle. 
		
		cxMath2 = xMathTocX(xMath2);
		cyMath2 = yMathTocY(yMath2);
		cxMath3 = xMathTocX(xMath3);
		cyMath3 = yMathTocY(yMath3);
		cxMath4 = xMathTocX(xMath4);
		cyMath4 = yMathTocY(yMath4);
		cxMath5 = xMathTocX(xMath5);
		cyMath5 = yMathTocY(yMath5);
		Duyball.FillEllipse(Brushes.White,(cxMath2-cRadius),(cyMath2 -cRadius),(2*5),(2*5));
		Duyball.FillEllipse(Brushes.Red,(cxMath3-cRadius),(cyMath3 -cRadius),(2*5),(2 *5));
		Duyball.FillEllipse(Brushes.White,(cxMath4-cRadius),(cyMath4 -cRadius),(2*5),(2 *5));
		Duyball.FillEllipse(Brushes.Red,(cxMath5-cRadius),(cyMath5 -cRadius),(2*5),(2 *5));
		}
	}
	
}