using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIFrame
{
    public class Agent : IComparable<Agent>
    {

        #region 属性

        /// <summary>
        /// 基因
        /// </summary>
        public Genotype Genotype
        {
            get;
            private set;
        }

        /// <summary>
        /// 神经网络
        /// </summary>
        public NeuralNetwork FNN
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 构造遗传代理
        /// </summary>
        /// <param name="_genotype">基因</param>
        /// <param name="_activation">激活函数</param>
        /// <param name="_topogy">拓扑结构</param>
        public Agent(Genotype _genotype, NeuralLayer.ActivationFunction _activation, params uint[] _topogy)
        {
            Genotype = _genotype;
            FNN = new NeuralNetwork(_topogy);
            foreach (NeuralLayer layer in FNN.Layers)
            {
                layer.NeuronActivationFunction = _activation;
            }
            if(FNN.WeightCount != _genotype.ParameterCount)
                throw new ArgumentException("WeightCount != Topology");
            //使用基因参数初始化神经网络
            IEnumerator<double> parameters = _genotype.GetEnumerator();
            foreach (NeuralLayer layer in FNN.Layers)
            {
                for (int i = 0; i < layer.Weights.GetLength(0); i++)
                {
                    for (int j = 0; j < layer.Weights.GetLength(1); j++)
                    {
                        layer.Weights[i, j] = parameters.Current;
                        parameters.MoveNext();
                    }
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            Genotype.Evaluation = 0;
            Genotype.Fitness = 0;
        }

        public int CompareTo(Agent other)
        {
            return Genotype.CompareTo(other.Genotype);
        }
       
        #endregion
    }
}
