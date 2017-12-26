using UnityEngine;

namespace AIFrame
{
    public abstract class Entity : MonoBehaviour
    {
        #region 属性

        //遗传代理
        public Agent Agent;

        /// <summary>
        /// 分数，需要根据需要在演化过程中更新 
        /// </summary>
        public float AgentScore
        {
            get { return Agent.Genotype.Evaluation; }
            set { Agent.Genotype.Evaluation = value; }
        }

        /// <summary>
        /// 是否存活，如果否，则不再执行演化函数
        /// </summary>
        public bool IsAlive = true;

        #endregion

        #region 抽象函数
        /// <summary>
        /// 设置输入
        /// </summary>
        /// <returns></returns>
        protected abstract float[] SetInputs();

        /// <summary>
        /// 应用输出
        /// </summary>
        protected abstract void GetOutPuts(float[] outputs);

        /// <summary>
        /// 当进化开始，停止各种状态
        /// </summary>
        public abstract void OnEvolutionBegin();

        /// <summary>
        /// 当进化结束，重启各种状态
        /// </summary>
        public abstract void OnEvolutionEnd();
        #endregion

        #region 函数

        /// <summary>
        /// 行为演化，使用神经网络处理输入信号
        /// </summary>
        public void GeneticUpdate()
        {
            float[] inputs = SetInputs();
            float[] outputs = Agent.FNN.ProcessInputs(inputs);
            GetOutPuts(outputs);
        }

        #endregion
    }
}
