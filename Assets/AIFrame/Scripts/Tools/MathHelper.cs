using System;
using Random = UnityEngine.Random;

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
            else return (float)(1.0 / (1.0 + Math.Exp(-_xValue)));
        }

        public static double SoftSignFunction(double xValue)
        {
            return xValue / (1 + Math.Abs(xValue));
        }

        /// <summary>
        /// 返回0-1之间的随机数
        /// </summary>
        /// <returns></returns>
        public static float RandomNext()
        {
            return Random.Range(0f, 1f);
        }
    }
}
