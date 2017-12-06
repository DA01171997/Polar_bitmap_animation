//File: Curvesfame.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: Curves
//Purpose: Drawing a polargraph of 8 petals flower by equation of r = sin(4theta) using bitmap
//Last modified: 12/6/2018
//Source Files in this program: CurvesAlgorithm.cs, Curvesframe.cs, Curvestmain.cs, run.sh
//Compile and execute program in Bash Shell. Type ./run.sh in to respective directory's terminal. 
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

public class Curvesframe : Form {
private const int xframe = 800; //UI X's size
	private const int yframe = 1000;//UI Y's size
	private const int pensize = 1;
	private const int cRadius =2;
	private const double refresh_rate = 30.0;//Hertz: How many times per second the display area is repainted.
	private const double ball_refresh_rate=50.0;
	private const double restart_clock_rate=1.0;
	private const double destroy_clock_rate=50.0;
	private const double time_converter=1000.0; //Timer Interval is in milisecond. This is 1 second
	private const double linear_velocity = 44.5;
	private const double spiral_rate = 50.0;
	private Pen ballpointpen;
	private Pen ballpointpenDash;
	private Font fontStyle = new Font("Arial", 10);
	private Font endfontStyle = new Font("Arial",25);
	private SolidBrush drawBrush = new SolidBrush(Color.Black);
	private Button startB = new System.Windows.Forms.Button();
	private Button exitB = new System.Windows.Forms.Button();
	private Point startBL = new Point(590,850);	
	private Point exitBL = new Point(650,850);		
	private Size buttonSize = new Size(60,50);
	private float speedD = 100;		//initial speed 40pix sec
	private float mathToCRatio=50;
	private float xMath=0;
	private float yMath=0;
	private float cxMath;
	private float cyMath;
	private String cCordMessage = "Pen location = ";
	private String ballspeedmessage = "Linear speed= ";
	private String degreemessage = "Theta = ";
	private String myName = "Duy Do";
	private String finishedMessage = "The curves is finished. Now it will redraws.";
	private String finishedMessage2 = "You may exit.";
	private bool finished = false;
	//Declare clocks
	private static System.Timers.Timer graphic_area_refresh_clock = new System.Timers.Timer();
	private static System.Timers.Timer spiral_clock = new System.Timers.Timer();
	private enum Spiral_clock_state_type{Begin,Ticking,Paused};
	private Spiral_clock_state_type spiral_state = Spiral_clock_state_type.Begin;
	
	//Declare Bitmap
	private System.Drawing.Graphics pointer_to_graphic_surface;
	private System.Drawing.Bitmap pointer_to_bitmap_in_memory = new Bitmap(xframe,yframe,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
	
	//Declare Algorithm Varibles
	Curves_algorithms polarCurves;
	private double theta=0; 
	private double distance_1tic;
	private double mathematical_distance_traveled_in_one_tic;
	private float curvesConstant =4.0f;
	private float bigPolarfactor=7;
	
	public Curvesframe () {
        // Initialize the UI
		Text = "Curves by Duy Do";
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
		exitB.BackColor = Color.Salmon;
		exitB.TabIndex = 2;
		
		startB.Text = "New Game";
		startB.Size = buttonSize;
		startB.Location = startBL;
		startB.BackColor = Color.Salmon;
		startB.TabIndex = 5;		
		
		// Add Buttons to frame
		Controls.Add(exitB);
		Controls.Add(startB);
		// Add live handlers to UI
		exitB.Click += new EventHandler(exitBP);
		startB.Click += new EventHandler(Manage_spiral_clock);
		
		// Prepare the refresh clock
		graphic_area_refresh_clock.Enabled = false;
		graphic_area_refresh_clock.Elapsed += new ElapsedEventHandler(Update_the_graphic_area);
		//Prepare the spiral clock
        spiral_clock.Enabled = false;
        spiral_clock.Elapsed += new ElapsedEventHandler(Update_the_position_of_the_spiral);
		
		//Use extra memory to make a smooth animation.
        DoubleBuffered = true;

		//Initialize the pointer used to write onto the bitmap stored in memory.
		pointer_to_graphic_surface = Graphics.FromImage(pointer_to_bitmap_in_memory);
		initialize_bitmap();
		
		// Define Algorithm variables
		polarCurves = new Curves_algorithms();
		distance_1tic = linear_velocity/spiral_rate;
		mathematical_distance_traveled_in_one_tic = distance_1tic/mathToCRatio;
		xMath=(float) (System.Math.Sin(curvesConstant*theta)*System.Math.Cos(theta)*bigPolarfactor);
		yMath=(float) (System.Math.Sin(curvesConstant*theta)*System.Math.Sin(theta)*bigPolarfactor);
		//Start refreshing the screen
		Start_graphic_clock(refresh_rate);
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
   
   protected void Start_graphic_clock(double refreshrate)						// Start Graphic Clock method
    {double elapsedtimebetweentics;
    if(refreshrate < 1.0) refreshrate = 1.0;  //Do not allow updates slower than 1 hertz.
    elapsedtimebetweentics = time_converter/refreshrate;  //elapsed time between tics has units milliseconds
    graphic_area_refresh_clock.Interval = (int)System.Math.Round(elapsedtimebetweentics);
    graphic_area_refresh_clock.Enabled = true;  //Start this clock ticking
    System.Console.WriteLine("Method Start_graphic_clock has terminated.");
   }
    protected void Update_the_graphic_area(System.Object sender, ElapsedEventArgs even)				//Screen refresh method
    {   System.Console.WriteLine("Refreshing Screen");
			  //check to see if the curves if finished drawing
			  if (theta<2*Math.PI){
			  pointer_to_graphic_surface.FillEllipse(Brushes.DarkTurquoise,(cxMath-cRadius),(cyMath -cRadius),(2* cRadius),(2 *cRadius));
			  }
			  else {
			  pointer_to_graphic_surface.FillEllipse(Brushes.Blue,(cxMath-cRadius),(cyMath -cRadius),(2* cRadius),(2 *cRadius));
			  finished=true;	//Curve finished drawing
			  }
		Invalidate();
   }
    protected void Update_the_position_of_the_spiral(System.Object sender,ElapsedEventArgs an_event)
     {//Call a method to compute the next pair of Cartesian coordinates for the moving particle tracing the path of the spiral.
		
		polarCurves.get_next_coordinates(curvesConstant, mathematical_distance_traveled_in_one_tic, bigPolarfactor,
                                      ref theta,
                                      out xMath,
                                      out yMath);
	  cxMath = xMathTocX(xMath);													// Convert math x to C# x
	  cyMath = yMathTocY(yMath);	
	  System.Console.WriteLine("New X and Y position");
	  Invalidate();
     }//End of method Update_the_position_of_the_spiral
   protected void Manage_spiral_clock(Object sender, EventArgs events)
   {switch(spiral_state)
       {case Spiral_clock_state_type.Begin: 
             double elapsed_time_between_updates_of_spiral_coordinates;
             double local_spiral_update_rate = spiral_rate;
             //In the next statement don't allow the spiral to update at a rate slower than 1.0 Hz
             if(local_spiral_update_rate < 1.0) local_spiral_update_rate = 1.0;
             elapsed_time_between_updates_of_spiral_coordinates = 1000.0/local_spiral_update_rate;
             spiral_clock.Interval = (int)System.Math.Round(elapsed_time_between_updates_of_spiral_coordinates);
             spiral_clock.Enabled = true;
             startB.Text = "Pause";
             spiral_state = Spiral_clock_state_type.Ticking;
             graphic_area_refresh_clock.Enabled = true;
             System.Console.WriteLine("Begin case finished executing");
             break;
        case Spiral_clock_state_type.Ticking: 
             spiral_clock.Enabled = false;
             spiral_state = Spiral_clock_state_type.Paused;
             startB.Text = "Go";
             graphic_area_refresh_clock.Enabled = false;
             System.Console.WriteLine("Ticking case finished executing");
             break;
        case Spiral_clock_state_type.Paused: 
             spiral_clock.Enabled = true;
             spiral_state = Spiral_clock_state_type.Ticking;
             startB.Text = "Pause";
             graphic_area_refresh_clock.Enabled = true;
             System.Console.WriteLine("Paused case finished executing");
             break;
        default:
             System.Console.WriteLine("A serious error occurred in the switch statement.");
             break;
       }//End of switch
   }//End of Manage_spiral_clock
   
   
   protected void initialize_bitmap()      {  
		pointer_to_graphic_surface.Clear(System.Drawing.Color.DeepPink);
		//Draw interface box
		pointer_to_graphic_surface.FillRectangle(Brushes.Peru,0, xframe, xframe-1, yframe-1);											
		//Draw The Axis
		pointer_to_graphic_surface.DrawLine(ballpointpenDash, 0, xframe/2, xframe/2, xframe/2);
		pointer_to_graphic_surface.DrawLine(ballpointpenDash, xframe/2,xframe/2, xframe/2, xframe);
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2, 0, xframe/2, xframe/2);
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2,xframe/2, xframe, xframe/2);
		//Initialize XY Coordinates
		for (int i=1; i<=xframe/2/mathToCRatio; i++){
		string temp = i.ToString();
		//Draw Numbers
		pointer_to_graphic_surface.DrawString(temp, fontStyle, drawBrush, xframe/2 + i*mathToCRatio, xframe/2 + 10);
		pointer_to_graphic_surface.DrawString(temp, fontStyle, drawBrush, xframe/2 + 10, xframe/2 - i*mathToCRatio);
		pointer_to_graphic_surface.DrawString("-"+temp, fontStyle, drawBrush, xframe/2 - i*mathToCRatio, xframe/2 + 10);
		pointer_to_graphic_surface.DrawString("-"+temp, fontStyle, drawBrush,xframe/2 + 10, xframe/2 + i*mathToCRatio );
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2-10, xframe/2 - i*mathToCRatio, xframe/2+10, xframe/2-i*mathToCRatio);
		//Draw Number Dashes
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2+i*mathToCRatio, xframe/2-10, xframe/2+i*mathToCRatio, xframe/2+10);
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2-10, xframe/2 - i*mathToCRatio, xframe/2+10, xframe/2-i*mathToCRatio);
		pointer_to_graphic_surface.DrawLine(ballpointpen, 0 + i*mathToCRatio, xframe/2 - 10, 0 + i*mathToCRatio, xframe/2 + 10);
		pointer_to_graphic_surface.DrawLine(ballpointpen, xframe/2-10, xframe/2 + i*mathToCRatio, xframe/2+10, xframe/2+i*mathToCRatio);
		//Draw Elipse Scale
		pointer_to_graphic_surface.DrawEllipse(ballpointpen, (xframe/2-i*mathToCRatio),(xframe/2-i*mathToCRatio),(2*i*mathToCRatio),(2*i*mathToCRatio));
		}
   }//End of initialize_bitmap()

   
	protected override void OnPaint(PaintEventArgs graph) {
		Graphics curves = graph.Graphics;																			// Convert math y to C# y
		//Draw Bitmap to OnPaint
		curves.DrawImage(pointer_to_bitmap_in_memory,0,0,xframe,yframe);
		//Draw String
		curves.DrawString(myName,fontStyle, drawBrush, 50,50);
		curves.DrawString(ballspeedmessage + speedD + " pix/sec", fontStyle, drawBrush, 275,810);
		curves.DrawString(cCordMessage+"(" + cxMath + "," + cyMath + ")", fontStyle, drawBrush, 275, 850);
		curves.DrawString(degreemessage + theta, fontStyle, drawBrush, 275,890);
		//Exit message when curves if finished 
		if (finished) { 
		curves.DrawString(finishedMessage, endfontStyle, drawBrush, 100 , xframe/4*3);
		curves.DrawString(finishedMessage2, endfontStyle, drawBrush, xframe*1/3+50, xframe/4*3+25);
		}
		base.OnPaint(graph);
		
	}
	
}