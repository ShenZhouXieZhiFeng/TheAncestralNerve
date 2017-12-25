using UnityEngine;

namespace AIFrame
{
    public abstract class Entity : MonoBehaviour
    {
        //遗传代理
        public Agent Agent;

        /// <summary>
        /// 遗传演化执行过程,根据演化速率每帧调多次
        /// </summary>
        public abstract void GeneticRunningUpdate();

        /// <summary>
        /// 当进化开始，停止各种状态
        /// </summary>
        public abstract void OnEvolutionBegin();

        /// <summary>
        /// 当进化结束，重启各种状态
        /// </summary>
        public abstract void OnEvolutionEnd();

    }
}
