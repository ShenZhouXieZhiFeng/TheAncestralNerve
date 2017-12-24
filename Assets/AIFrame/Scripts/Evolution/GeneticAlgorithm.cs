using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFrame
{
    /// <summary>
    /// 遗传算法
    /// </summary>
    public partial class GeneticAlgorithm
    {

        #region 遗传参数设定

        //默认初始化神经的最小值
        public const float DefInitParamMin = -1.0f;

        //默认初始化神经的最大值
        public const float DefInitParamMax = 1.0f;

        //默认的种群杂交的概率
        public const float DefCrossSwapProb = 0.6f;

        //默认的基于突变概率
        public const float DefMutationProb = 0.1f;

        //默认的突变参数
        public const float DefMutationAmount = 2.0f;

        //默认的突变百分比
        public const float DefMutationPerc = 1.0f;

        #endregion

        #region 遗传方法委托定义

        public delegate void InitialisationOperator(IEnumerable<Genotype> initialPopulation);

        public delegate void EvaluationOperator(IEnumerable<Genotype> currentPopulation);

        #endregion

        #region 遗传操作方法设定

        /// <summary>
        /// 初始化种群
        /// </summary>
        public InitialisationOperator InitialisePopulation = DefaultPopulationInitialisation;

        /// <summary>
        /// 评估种群
        /// </summary>
        public EvaluationOperator Evaluation = AsyncEvaluation;

        #endregion

        #region 属性

        private List<Genotype> currentPopulation;

        /// <summary>
        /// 种群规模
        /// </summary>
        public uint PopulationSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 遗传次数
        /// </summary>
        public uint GenerationCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 在选择进化之前是否进行种群排序
        /// </summary>
        public bool SortPopulation
        {
            get;
            private set;
        }

        /// <summary>
        /// 遗传流程是否正在执行
        /// </summary>
        public bool Running
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 初始化一个算法控制类
        /// </summary>
        /// <param name="_genotypeParamCount"></param>
        /// <param name="_populationSize"></param>
        public GeneticAlgorithm(uint _genotypeParamCount,uint _populationSize)
        {
            PopulationSize = _populationSize;

            currentPopulation = new List<Genotype>((int)_populationSize);

            for (int i = 0; i < _populationSize; i++)
            {
                currentPopulation.Add(new Genotype(new double[_genotypeParamCount]));
            }

            GenerationCount = 1;
            SortPopulation = true;
            Running = false;
        }

        public void Start()
        {
            Running = true;
            InitialisePopulation(currentPopulation);

        }

        #endregion
        
    }
}
