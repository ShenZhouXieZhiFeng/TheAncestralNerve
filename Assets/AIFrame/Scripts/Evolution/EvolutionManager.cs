using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFrame
{
    /// <summary>
    /// 遗传管理
    /// </summary>
    public class EvolutionManager : SingletonMono<EvolutionManager>
    {
        #region 属性

        [Header("种群数量")]
        public int PopulationSize = 30;

        [Header("遗传次数上限")]
        public int GeneticCountlimit = 100;

        [Header("神经网络拓扑结构")]
        public uint[] FNNTopology;

        //当前存在的代理集合
        private List<Agent> agents = new List<Agent>();

        //本次遗传存活的代理数量
        public int AgentsAliveCount
        {
            get;
            private set;
        }

        //遗传算法
        private GeneticAlgorithm geneticAlgorithm;

        //从开始到现在的进化次数
        public uint GenerationCount
        {
            get { return geneticAlgorithm.GenerationCount; }
        }

        #endregion

        #region UNITY

        void Start()
        {
            
        }

        #endregion

        #region 方法

        /// <summary>
        /// 开始遗传
        /// </summary>
        public void StartGenetic()
        {
            //初始化神经网络
            NeuralNetwork nn = new NeuralNetwork(FNNTopology);
            //初始化遗传算法
            geneticAlgorithm = new GeneticAlgorithm((uint)nn.WeightCount, (uint)PopulationSize);
            //设定评估方法
            geneticAlgorithm.Evaluation = Evaluation;
            //启动遗传过程
            geneticAlgorithm.Start();
        }

        public void Evolution()
        {
            
        }

        /// <summary>
        /// 评估
        /// </summary>
        void Evaluation(IEnumerable<Genotype> currentPopulation)
        {

        }

        #endregion
    }
}
