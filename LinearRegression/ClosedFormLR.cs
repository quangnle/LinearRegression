using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearRegression
{
    public class ClosedFormLR
    {
        /// <summary>
        /// Get a closed form solution to linear regression
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Matrix<double> Regression(Matrix<double> x, Matrix<double> y)
        {
            var theta = (x.Transpose() * x).Inverse() * x.Transpose() * y;
            return theta;
        }
    }
}
