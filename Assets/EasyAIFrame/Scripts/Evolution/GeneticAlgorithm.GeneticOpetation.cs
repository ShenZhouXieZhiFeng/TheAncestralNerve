using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAIFrame
{
    /// <summary>
    /// 遗传过程中的各种操作
    /// </summary>
    public partial class GeneticAlgorithm
    {
        #region 遗传方法委托声明

        public delegate void InitialisationOperator(IEnumerable<Genotype> initialPopulation);

        public delegate void EvaluationOperator(List<Genotype> _newGenotypes);

        public delegate void FitnessCalculation(IEnumerable<Genotype> currentPopulation);

        public delegate List<Genotype> SelectionOperator(List<Genotype> currentPopulation);

        public delegate List<Genotype> RecombinationOperator(List<Genotype> intermediatePopulation, uint newPopulationSize);

        public delegate void MutationOperator(List<Genotype> newPopulation);

        public delegate bool CheckTerminationCriterion(IEnumerable<Genotype> currentPopulation);

        #endregion

        #region 遗传操作方法声明

        #region 不可替换的方法

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
        /// 检查是否达到遗传停止的条件
        /// </summary>
        public CheckTerminationCriterion TerminationCriterion = null;

        /// <summary>
        /// 当遗传停止时
        /// </summary>
        public Action AlgorithmTerminated;

        #endregion

        #region 可替换的

        /// <summary>
        /// 选择遗传体
        /// </summary>
        public SelectionOperator Selection = RouletteWheelsSelectionOperator;//DefaultSelectionOperator;

        /// <summary>
        /// 基因交叉操作
        /// </summary>
        public RecombinationOperator Recombination = DefaultRecombinationOperator;

        /// <summary>
        /// 基因变异操作
        /// </summary>
        public MutationOperator Mutation = DefaultMutationOperator;

        #endregion

        #endregion

        #region 遗传操作方法

        /// <summary>
        /// 默认的种群初始化函数
        /// </summary>
        /// <param name="population"></param>
        public static void DefaultPopulationInitialisation(IEnumerable<Genotype> population)
        {
            //将参数设置为设置范围中的随机值
            foreach (Genotype genotype in population)
                genotype.SetRandomParameters(DefInitParamMin, DefInitParamMax);
        }

        /// <summary>
        /// 计算种群中个体的适应性:
        /// </summary>
        public static void DefaultFitnessCalculation(IEnumerable<Genotype> currentPopulation)
        {
            //统计总评分
            uint populationSize = 0;
            float overallEvaluation = 0;
            foreach (Genotype genotype in currentPopulation)
            {
                overallEvaluation += genotype.Evaluation;
                populationSize++;
            }
            //计算平均评分
            float averageEvaluation = overallEvaluation / populationSize;
            //计算每个个体的适应性
            foreach (Genotype genotype in currentPopulation)
                genotype.Fitness = genotype.Evaluation / averageEvaluation;
        }

        /// <summary>
        /// 评估
        /// </summary>
        /// <param name="currentPopulation"></param>
        public static void AsyncEvaluation(IEnumerable<Genotype> currentPopulation)
        {
            //这时应该启动异步评估，在完成评估之后，应该调用它
        }

        /// <summary>
        /// 默认的选择父母的方法，直接精英选择
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        public static List<Genotype> DefaultSelectionOperator(List<Genotype> currentPopulation)
        {
            //直接取健壮性最高的前2个基因型
            List<Genotype> intermediatePopulation = new List<Genotype>();
            intermediatePopulation.Add(currentPopulation[0]);
            intermediatePopulation.Add(currentPopulation[1]);

            return intermediatePopulation;
        }

        /// <summary>
        /// 轮赌盘筛选法,分数越高，选中的可能性越高
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        public static List<Genotype> RouletteWheelsSelectionOperator(List<Genotype> currentPopulation)
        {
            List<Genotype> intermediatePopulation = new List<Genotype>();

            float totalEva = 0;
            foreach (Genotype geno in currentPopulation)
            {
                totalEva += geno.Evaluation;
            }
            //随机选两个
            for (int i = 0; i < 2; i++)
            {
                float random = MathHelper.RandomNext() * totalEva;
                float tempSum = 0;
                foreach (Genotype geno in currentPopulation)
                {
                    tempSum += geno.Evaluation;
                    if (tempSum >= random)
                    {
                        intermediatePopulation.Add(geno);
                        break;
                    }
                }
            }
            return intermediatePopulation;
        }

        /// <summary>
        /// 默认的交叉函数
        /// </summary>
        public static List<Genotype> DefaultRecombinationOperator(List<Genotype> intermediatePopulation, uint newPopulationSize)
        {
            if (intermediatePopulation.Count < 2) throw new ArgumentException("intermediatePopulation count < 2");
            //生成新的基因型
            List<Genotype> newPopulation = new List<Genotype>();
            while (newPopulation.Count < newPopulationSize)
            {
                Genotype offspring1, offspring2;
                //将基因0和1，交换参数，进行叉乘操作，返回两个新的基因型
                CompleteCrossover(intermediatePopulation[0], intermediatePopulation[1], DefCrossSwapProb, out offspring1, out offspring2);
                //将新的基因型进行保存
                newPopulation.Add(offspring1);
                if (newPopulation.Count < newPopulationSize)
                    newPopulation.Add(offspring2);
            }
            return newPopulation;
        }

        /// <summary>
        /// 基因交叉操作
        /// </summary>
        public static void CompleteCrossover(Genotype parent1, Genotype parent2, float swapChance, out Genotype offspring1, out Genotype offspring2)
        {
            int parameterCount = parent1.ParameterCount;
            float[] off1Parameters = new float[parameterCount];
            float[] off2Parameters = new float[parameterCount];

            //遍历所有参数，随机交换
            for (int i = 0; i < parameterCount; i++)
            {
                //如果生成的随机数小于交换参考值，则进行交换
                if (MathHelper.RandomNext() < swapChance)
                {
                    //交互参数
                    off1Parameters[i] = parent2[i];
                    off2Parameters[i] = parent1[i];
                }
                else
                {
                    //不进行交换，直接遗传到子基因
                    off1Parameters[i] = parent1[i];
                    off2Parameters[i] = parent2[i];
                }
            }
            //根据新的参数矩阵生成新的基于型
            offspring1 = new Genotype(off1Parameters);
            offspring2 = new Genotype(off2Parameters);
        }

        /// <summary>
        /// 默认的基因变异函数
        /// </summary>
        /// <param name="newPopulation"></param>
        public static void DefaultMutationOperator(List<Genotype> newPopulation)
        {
            foreach (Genotype genotype in newPopulation)
            {
                if (MathHelper.RandomNext() < DefMutationPerc)
                    MutateGenotype(genotype, DefMutationProb, DefMutationAmount);
            }
        }

        /// <summary>
        /// 基因变异操作
        /// </summary>
        public static void MutateGenotype(Genotype genotype, float mutationProb, float mutationAmount)
        {
            //mutationProb 一个参数被突变的概率
            //mutationAmount 默认的变异参数
            for (int i = 0; i < genotype.ParameterCount; i++)
            {
                if (MathHelper.RandomNext() < mutationProb)
                {
                    //Mutate by random amount in range [-mutationAmount, mutationAmoun]
                    //控制变异后的参数在一定的量内
                    genotype[i] += (float)(MathHelper.RandomNext() * (mutationAmount * 2) - mutationAmount);
                }
            }
        }

        #endregion
    }
}