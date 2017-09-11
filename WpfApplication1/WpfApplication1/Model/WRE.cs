using System;
using System.Runtime.InteropServices;

namespace AppelWRE
{
    public class WRE
    {
        // import WRE dlls
        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCov", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingCov(
            ref int returnsSize,
            ref int nbSec,
            double[,] secReturns,
            double[,] covMatrix,
            ref int info
        );

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREanalysisExpostVolatility", CallingConvention = CallingConvention.Cdecl)]

        // declaration
      
        public static extern int WREanalysisExpostVolatility(
            ref int sizeMxCov,
            double[,] returnMatrix,
            double[,] volatility,
            ref int info
        );

        [DllImport("wre-ensimag-c-4.1.dll", EntryPoint = "WREmodelingCorr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int WREmodelingCorr(
          ref int nbValues,
          ref int nbAssets,
          double[,] inputValues,
          double[,] corrMatrix,
          ref int info
      );
        public static double[,] computeCorrelationMatrix(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] corrMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCorr(ref dataSize, ref nbAssets, returns, corrMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCorr encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCorr encountred a problem. See info parameter for more details");
            }
            return corrMatrix;
        }




        public static double[,] computeVolatility(double[,] returns)
        {
            int dataSize = returns.GetLength(0);
            double[,] volatility = new double[1, 1];
            int info = 0;
            int res;
            res = WREanalysisExpostVolatility(ref dataSize, returns, volatility, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: Volatility encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: Volatility encountred a problem. See info parameter for more details");
            }
            return volatility;
        }




        public static double[,] computeCovarianceMatrix(double[,] returns) //Carefull : unbiased variance (*n/n-1)
        {
            int dataSize = returns.GetLength(0);
            int nbAssets = returns.GetLength(1);
            double[,] covMatrix = new double[nbAssets, nbAssets];
            int info = 0;
            int res;
            res = WREmodelingCov(ref dataSize, ref nbAssets, returns, covMatrix, ref info);
            if (res != 0)
            {
                if (res < 0)
                    throw new Exception("ERROR: WREmodelingCov encountred a problem. See info parameter for more details");
                else
                    throw new Exception("WARNING: WREmodelingCov encountred a problem. See info parameter for more details");
            }
            return covMatrix;
        }

        public static void dispMatrix(double[,] myCovMatrix)
        {
            int n = myCovMatrix.GetLength(0);
            int m = myCovMatrix.GetLength(1);

            Console.WriteLine("Covariance matrix:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(myCovMatrix[i, j] + "\t");
                }
                Console.Write("\n");
            }
        }
        
    }
}