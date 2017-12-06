//File: CurvesAlgorithm.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: Curves





public class Curves_algorithms
{   private double magnitude_of_tangent_vector_squared;
    private double magnitude_of_tangent_vector;
	private double derivativeX;
	private double derivativeY;
    //Note that all values in the method below are mathematical units.  None of the values have been scaled for output by a computer.
    public void get_next_coordinates(float curvesConstant,          //Constant 'a' in the equation r = a+b*t
                                     double mathematical_distance_traveled_in_one_tic,           //Constant 'b' in the equation r = a+b*       //The distance the brush will move in one tic of the spiral clock.
                                     ref double theta,                   //The variable 't' in the equation r = a+b*t
                                     out float xMath,               //The next x coordinate of the spiral
                                     out float yMath)               //The next y coordinate of the spiral
       {	
	   derivativeX = (double)(System.Math.Cos(theta)*System.Math.Cos(curvesConstant*theta)*curvesConstant *7 - System.Math.Sin(theta) * System.Math.Sin(curvesConstant*theta)*7);
		derivativeY = (double)(System.Math.Cos(theta)*System.Math.Sin(curvesConstant*theta)*7 + curvesConstant*System.Math.Sin(theta)*System.Math.Cos(curvesConstant*theta)*7);   
		//erivativeX = (double)(-2*System.Math.Cos(theta)*System.Math.Sin(curvesConstant*theta) - System.Math.Sin(theta)*System.Math.Cos(curvesConstant*theta));
		//derivativeY = (double)(System.Math.Cos(theta)*System.Math.Cos(curvesConstant*theta) - curvesConstant*System.Math.Sin(theta)*System.Math.Sin(curvesConstant*theta));
		// derivativeX = (double) (System.Math.Cos(theta) * System.Math.Cos(theta) - System.Math.Sin(theta) * System.Math.Sin(theta));
		// derivativeY = (double) (System.Math.Cos(theta) * System.Math.Sin(theta) *2);
		
		magnitude_of_tangent_vector_squared=derivativeX*derivativeX + derivativeY *derivativeY;
        magnitude_of_tangent_vector = System.Math.Sqrt(magnitude_of_tangent_vector_squared);
		System.Console.WriteLine(" Theta Old: " + theta);
        theta = theta + mathematical_distance_traveled_in_one_tic/magnitude_of_tangent_vector;
		System.Console.WriteLine(" Theta New: " + theta);
		xMath=(float) (System.Math.Sin(curvesConstant*theta)*System.Math.Cos(theta)*7);
		yMath=(float) (System.Math.Sin(curvesConstant*theta)*System.Math.Sin(theta)*7);
		//xMath=(float) (System.Math.Cos(curvesConstant*theta)*System.Math.Cos(theta));
		//yMath=(float) (System.Math.Cos(curvesConstant*theta)*System.Math.Sin(theta));
		//xMath= (float) (System.Math.Sin(theta)* System.Math.Cos(theta));
       //yMath= (float) (System.Math.Sin(theta)* System.Math.Sin(theta));
	   }//End of get_next_coordinates
       
}//End  of class Curves_algorithms