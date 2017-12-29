using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAIFrame
{
    /// <summary>
    /// 遗传算法
    /// </summary>
    public partial class GeneticAlgorithm
    {

        #region 遗传参数设定

        //默认初始化神经的最小值
        public static float DefInitParamMin = -1.0f;

        //默认初始化神经的最大值
        public static float DefInitParamMax = 1.0f;

        //默认的种群杂交的概率
        public static float DefCrossSwapProb = 0.6f;

        //默认的基于突变概率
        public static float DefMutationProb = 0.1f;

        //默认的突变参数
        public static float DefMutationAmount = 2.0f;

        //默认的突变百分比
        public static float DefMutationPerc = 1.0f;

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
        /// 遗传流程是否正在执行
        /// </summary>
        public bool Running
        {
            get;
            set;
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
            //初始化种群
            PopulationSize = _populationSize;
            currentPopulation = new List<Genotype>((int)_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                currentPopulation.Add(new Genotype(new float[_genotypeParamCount]));
            }
            //初始化参数
            GenerationCount = 1;
            Running = false;
        }

        /// <summary>
        /// 启动遗传进程
        /// </summary>
        public void Start()
        {
            Running = true;
            InitialisePopulation(currentPopulation);
            Evaluation(currentPopulation);
        }

        /// <summary>
        /// 暂停演化过程，开始评估进化
        /// </summary>
        public void StartEvaluation()
        {
            Running = false;
            //计算适应性
            FitnessCalculationMethod(currentPopulation);
            //根据适应性排序
            currentPopulation.Sort();

            if (TerminationCriterion != null && TerminationCriterion(currentPopulation))
            {
                if(AlgorithmTerminated != null)
                    AlgorithmTerminated();
                return;
            }
            //根据设定的筛选函数选中能遗传给下代的基因型
            List<Genotype> parentPopulation = Selection(currentPopulation);

            //交叉
            List<Genotype> newPopulation = Recombination(parentPopulation,PopulationSize);

            //突变
            Mutation(newPopulation);

            //设定为新的种群，重启遗传过程
            currentPopulation = newPopulation;
            GenerationCount++;

            Evaluation(currentPopulation);
        }

        #endregion

    }
}
