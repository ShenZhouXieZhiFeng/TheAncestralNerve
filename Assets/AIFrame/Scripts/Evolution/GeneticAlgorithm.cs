using System;
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

        public delegate void EvaluationOperator(List<Genotype> _newGenotypes);

        public delegate void FitnessCalculation(IEnumerable<Genotype> currentPopulation);

        public delegate List<Genotype> SelectionOperator(List<Genotype> currentPopulation);

        public delegate List<Genotype> RecombinationOperator(List<Genotype> intermediatePopulation, uint newPopulationSize);

        public delegate void MutationOperator(List<Genotype> newPopulation);

        public delegate bool CheckTerminationCriterion(IEnumerable<Genotype> currentPopulation);

        #endregion

        #region 遗传操作方法设定

        /// <summary>
        /// 初始化种群
        /// </summary>
        public InitialisationOperator InitialisePopulation = DefaultPopulationInitialisation;

        /// <summary>
        /// 计算种群的适应性
        /// </summary>
        public FitnessCalculation FitnessCalculationMethod = DefaultFitnessCalculation;

        /// <summary>
        /// 评估种群,循环的两个环节中的一个
        /// </summary>
        public EvaluationOperator Evaluation = AsyncEvaluation;

        /// <summary>
        /// 选择遗传体
        /// </summary>
        public SelectionOperator Selection = DefaultSelectionOperator;

        /// <summary>
        /// 基因交叉操作
        /// </summary>
        public RecombinationOperator Recombination = DefaultRecombinationOperator;

        /// <summary>
        /// 基因变异操作
        /// </summary>
        public MutationOperator Mutation = DefaultMutationOperator;

        /// <summary>
        /// 检查是否达到遗传停止的条件
        /// </summary>
        public CheckTerminationCriterion TerminationCriterion = null;

        /// <summary>
        /// 当遗传停止时
        /// </summary>
        public Action AlgorithmTerminated;
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
                currentPopulation.Add(new Genotype(new double[_genotypeParamCount]));
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
