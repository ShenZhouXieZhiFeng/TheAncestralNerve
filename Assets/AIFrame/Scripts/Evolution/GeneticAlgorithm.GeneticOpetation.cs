using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIFrame
{
    /// <summary>
    /// 遗传过程中的各种操作
    /// </summary>
    public partial class GeneticAlgorithm
    {
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

        public static void AsyncEvaluation(IEnumerable<Genotype> currentPopulation)
        {
            //这时应该启动异步评估，在完成评估之后，应该调用它
        }

        #endregion
    }
}