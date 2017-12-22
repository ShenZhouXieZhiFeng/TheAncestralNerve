using System;

namespace AIFrame
{
    public class MathHelper
    {
        /// <summary>
        /// Sigmoid激活函数
        /// </summary>
        public static double SigmoidFunction(double _xValue)
        {
            if (_xValue > 10) return 1.0f;
            else if (_xValue < -10) return 0.0f;
            else return (1.0 / (1.0 + Math.Exp(-_xValue)));
        }
    }
}
