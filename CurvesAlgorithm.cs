//File: CurvesAlgorithm.cs
//Author: Duy Do
//Author's email: duy.ado@fullerton.edu
//Author's Github: https://github.com/DA01171997
//Project: Curves





public class Curves_algorithms
{   private double magnitude_of_tangent_vector_squared;
    private double magnitude_of_tangent_vector;
    //Note that all values in the method below are mathematical units.  None of the values have been scaled for output by a computer.
    public void get_next_coordinates(double initial_radius,          //Constant 'a' in the equation r = a+b*t
                                     double b_coefficient,           //Constant 'b' in the equation r = a+b*t
                                     double distance_1_tic,          //The distance the brush will move in one tic of the spiral clock.
                                     ref double t,                   //The variable 't' in the equation r = a+b*t
                                     out double new_x,               //The next x coordinate of the spiral
                                     out double new_y)               //The next y coordinate of the spiral
       {magnitude_of_tangent_vector_squared = b_coefficient*b_coefficient + (initial_radius+b_coefficient*t)*(initial_radius+b_coefficient*t);
        magnitude_of_tangent_vector = System.Math.Sqrt(magnitude_of_tangent_vector_squared);
        t = t + distance_1_tic/magnitude_of_tangent_vector;
        new_x = (initial_radius + b_coefficient*t)*System.Math.Cos(t);
        new_y = (initial_radius + b_coefficient*t)*System.Math.Sin(t);
       }//End of get_next_coordinates
       
}//End  of class Curves_algorithms