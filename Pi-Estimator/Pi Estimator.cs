using System;

namespace Pi_Estimator
{
    public static class PiEstimator
    {
        public static double EstimatePiTo(int numDecimalPlaces, double largestDeviationFromPiAllowed, int numIterationsSoFar = 0, double previousEstimate = 0.0, double denominatorOfCurrentTerm = 1)
        {
            const int ConstantMultiplier = 4;

            bool numIterationsSoFarIsEven = numIterationsSoFar % 2 == 0;
            double currentEstimateNotMultipledByConstant = numIterationsSoFarIsEven ? previousEstimate + (1 / denominatorOfCurrentTerm) : previousEstimate - (1 / denominatorOfCurrentTerm);
            double currentEstimateMultipledByConstant = ConstantMultiplier * currentEstimateNotMultipledByConstant;

            if (Math.Abs(currentEstimateMultipledByConstant - Math.PI) <= largestDeviationFromPiAllowed)
            {
                return Math.Round(currentEstimateMultipledByConstant, numDecimalPlaces);
            }

            return EstimatePiTo(numDecimalPlaces, largestDeviationFromPiAllowed, numIterationsSoFar + 1, currentEstimateNotMultipledByConstant, denominatorOfCurrentTerm + 2);            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PiEstimator.EstimatePiTo(numDecimalPlaces: 5, largestDeviationFromPiAllowed: 0.05));

            Console.WriteLine("Enter any key to exit the program: ");
            Console.ReadKey();
        }
    }
}
