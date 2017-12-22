using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace AIFrame
{
    /// <summary>
    /// 神经层
    /// </summary>
    public class NeuralLayer
    {
        #region 属性

        /// <summary>
        /// 激活函数
        /// </summary>
        public delegate double ActivationFunction(double xValue);
        public ActivationFunction NeuronActivationFunction = MathHelper.SigmoidFunction;

        /// <summary>
        /// 偏置
        /// </summary>
        private float bias = 1.0f;

        /// <summary>
        /// 传进来的激活值或数据的数量
        /// </summary>
        public uint NeuronNodeCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 传给下一层的数据的数量
        /// </summary>
        public uint OutputCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 权重矩阵,行数表示输入的数据量（个数），列数为输出的数据量
        /// </summary>
        public double[,] Weights
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_nodeCount">节点数量，可以理解为上一层传下来的激活值的数量</param>
        /// <param name="_outputCount">传递给下一层的激活值的数量</param>
        public NeuralLayer(uint _nodeCount,uint _outputCount)
        {
            NeuronNodeCount = _nodeCount;
            OutputCount = _outputCount;
            //+1为偏置b
            Weights = new double[_nodeCount + 1, _outputCount];
        }

        /// <summary>
        /// 加权计算
        /// 可以理解为矩阵计算，输入在左边为行向量，输出为列向量，输出的个数取决于中间矩阵的列数（即OutputCount）
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public double[] ProcessInputs(double[] inputs)
        {
            //矩阵计算时，左边矩阵的列数必须等于右边矩阵的行数
            if (inputs.Length != NeuronNodeCount)
                throw new ArgumentException("inputs length != node Count");

            double[] sums = new double[OutputCount];
            //将b设置为1，偏置用来影响激活函数的输出
            double[] biasedInputs = new double[NeuronNodeCount + 1];
            inputs.CopyTo(biasedInputs, 0);
            biasedInputs[inputs.Length] = bias;

            //加权计算 
            //此时这一层的权重是一个矩阵，计算方式应为：(i[0]为输入的第一个值，w为权重矩阵)
            //i[0]*w[0,0] + i[1] * w[1,0] + i[2] * w[2,0] + ...
            //Weights.GetLength(0)行数，Weights.GetLength(1)列数
            for (int i = 0; i < Weights.GetLength(1); i++)
            {
                for (int j = 0; j < Weights.GetLength(0); j++)
                {
                    sums[i] += biasedInputs[i] * Weights[i, j];
                }
            }

            //应用激活函数
            if (NeuronActivationFunction != null)
            {
                for (int i = 0; i < sums.Length; i++) {
                    sums[i] = NeuronActivationFunction(sums[i]);
                }
            }

            return sums;
        }

        #endregion
    }
}
