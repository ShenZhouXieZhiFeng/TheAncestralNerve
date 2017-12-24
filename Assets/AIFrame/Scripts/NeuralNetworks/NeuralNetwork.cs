using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFrame
{
    /// <summary>
    /// 前馈神经网络
    /// </summary>
    public class NeuralNetwork
    {

        #region 属性

        /// <summary>
        /// 神经层
        /// </summary>
        public NeuralLayer[] Layers
        {
            get;
            private set;
        }

        /// <summary>
        /// 拓扑结构
        /// </summary>
        public uint[] Topology
        {
            get;
            private set;
        }

        /// <summary>
        /// 该神经网络中所有神经节点的数量
        /// </summary>
        public int WeightCount
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_topology">拓扑结构，如:4 3 3 2表示4个输入，2个输出，隐藏层为前3层</param>
        public NeuralNetwork(params uint[] _topology) 
        {
            Topology = _topology;

            WeightCount = 0;
            for (int i = 0; i < _topology.Length - 1; i++)
            {
                //(_topology[i] + 1) 偏置节点
                WeightCount += (int)((_topology[i] + 1) * _topology[i + 1]);
            }
            //初始化各神经层
            Layers = new NeuralLayer[_topology.Length + 1];
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new NeuralLayer(_topology[i], _topology[i + 1]);
            }
        }

        /// <summary>
        /// 使用该神经网络处理输入信号
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public double[] ProcessInputs(double[] _inputs)
        {
            if (_inputs.Length != Layers[0].NeuronNodeCount)
                throw new ArgumentException("input length != first layer count");

            double[] outputs = _inputs;
            //层层处理
            foreach (NeuralLayer layer in Layers)
            {
                outputs = layer.ProcessInputs(outputs);
            }

            return outputs;
        }

        #endregion

    }
}
