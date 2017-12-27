using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAIFrame
{
    /// <summary>
    /// 遗传管理
    /// </summary>
    public class EvolutionManager : SingletonMono<EvolutionManager>
    {
        #region 回调事件

        /// <summary>
        /// 当遗传开始
        /// </summary>
        public Action OnGeneticBeginAction;

        /// <summary>
        /// 当遗传结束
        /// </summary>
        public Action OnGeneticEndAction;

        /// <summary>
        /// 当新一轮的遗传演化开始,做一些实体复位之类的操作
        /// </summary>
        public Action<uint> OnGeneticRestartAction;

        /// <summary>
        /// 遗传更新中的回调，用来做一些进度条的显示等
        /// </summary>
        public Action<float> OnGeneticUpdateAction;

        #endregion

        #region 属性

        [Header("种群数量")]
        public int PopulationSize = 30;

        [Header("遗传次数上限")]
        public int GeneticCountlimit = 100;

        [Header("每次遗传的时间(秒)")]
        public float GeneticTimePerTimes = 30f;

        [Header("输入参数个数")]
        public uint InputParameterCount = 4;

        [Header("输出参数个数")]
        public uint OutPutParameterCount = 2;

        [Header("隐藏层层数")]
        public uint HiddenLayerCount = 2;

        [Header("每一层的神经节点数量")]
        public uint NeuronCountPerLayer = 5;

        //神经网络拓扑结构
        private uint[] FNNTopology;

        //当前存在的代理集合
        private List<Entity> entitys = new List<Entity>();

        //遗传算法
        private GeneticAlgorithm geneticAlgorithm;

        //从开始到现在的进化次数
        public uint GenerationCount
        {
            get { return geneticAlgorithm == null ? 0 : geneticAlgorithm.GenerationCount; }
        }

        private bool isBegin = false;

        #endregion

        #region UNITY

        protected override void Awake()
        {
            base.Awake();
            setTopology();
        }

        //当前遗传进度
        public float currentTime = 0;
        private void Update()
        {
            if (geneticAlgorithm == null)
                return;
            if (isBegin && geneticAlgorithm.Running)
            {
                currentTime += Time.deltaTime;
                if (currentTime > GeneticTimePerTimes)
                {
                    EndCurrentGenetic();
                    currentTime = 0;
                    return;
                }
                //执行更新回调
                if (OnGeneticUpdateAction != null)
                {
                    OnGeneticUpdateAction(currentTime / GeneticTimePerTimes);
                }
                //执行每个实体的更新函数
                foreach (Entity entity in entitys)
                {
                    entity.GeneticUpdate();
                }
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 构造神经网络拓扑结构
        /// </summary>
        void setTopology()
        {
            FNNTopology = new uint[HiddenLayerCount + 2];
            FNNTopology[0] = InputParameterCount;
            for (int i = 1; i <= HiddenLayerCount; i++)
            {
                FNNTopology[i] = NeuronCountPerLayer;
            }
            FNNTopology[FNNTopology.Length - 1] = OutPutParameterCount;
        }

        /// <summary>
        /// 开始遗传
        /// </summary>
        /// <param name="_entitys">实体集合</param>
        public void StartGenetic(List<Entity> _entitys)
        {
            if (_entitys.Count != PopulationSize)
                throw new ArgumentException("Entity Count != PopulationSize");
            //设置实体
            entitys = _entitys;

            isBegin = true;
            //初始化神经网络
            NeuralNetwork nn = new NeuralNetwork(FNNTopology);
            //初始化遗传算法
            geneticAlgorithm = new GeneticAlgorithm((uint)nn.WeightCount, (uint)PopulationSize);
            //设定评估方法
            geneticAlgorithm.Evaluation = Evaluation;
            //设定检查停止条件的方法
            geneticAlgorithm.TerminationCriterion += CheckGenerationTermination;
            //设定遗传停止时的方法
            geneticAlgorithm.AlgorithmTerminated += OnGATermination;
            //启动遗传过程
            geneticAlgorithm.Start();

            if (OnGeneticBeginAction != null)
                OnGeneticBeginAction();
        }

        /// <summary>
        /// 停止本次遗传演化，开始进化
        /// </summary>
        void EndCurrentGenetic()
        {
            foreach (Entity entity in entitys)
            {
                entity.OnEvolutionBegin();
            }
            geneticAlgorithm.StartEvaluation();
        }

        /// <summary>
        /// 评估
        /// </summary>
        void Evaluation(List<Genotype> _newGenotypes)
        {
            if (_newGenotypes.Count != entitys.Count)
                throw new ArgumentException("NewPopulation count != Entitys Count");
            //生成新的代理
            List<Agent> agents = new List<Agent>();
            for (int i = 0; i < _newGenotypes.Count; i++)
            {
                agents.Add(new Agent(_newGenotypes[i], MathHelper.SoftSignFunction, FNNTopology));
            }
            //为所有的实体设置新的代理
            for (int i = 0; i < entitys.Count; i++)
            {
                entitys[i].Agent = agents[i];
            }
            startNewGenetic();
        }

        /// <summary>
        /// 开始新的遗传进程
        /// </summary>
        void startNewGenetic()
        {
            foreach (Entity entity in entitys)
            {
                entity.OnEvolutionEnd();
            }
            if (OnGeneticRestartAction != null)
                OnGeneticRestartAction(geneticAlgorithm.GenerationCount);
            geneticAlgorithm.Running = true;
        }

        /// <summary>
        /// 检查是否满足停止条件
        /// </summary>
        /// <param name="currentPopulation"></param>
        /// <returns></returns>
        bool CheckGenerationTermination(IEnumerable<Genotype> currentPopulation)
        {
            return geneticAlgorithm.GenerationCount >= GeneticCountlimit;
        }

        /// <summary>
        /// 当遗传停止时被调用
        /// </summary>
        void OnGATermination()
        {
            if (OnGeneticEndAction != null)
                OnGeneticEndAction();
            isBegin = false;
            CancelInvoke("EndCurrentGenetic");
            Debug.Log("遗传停止");
        }

        public void Print(object msg)
        {
            Debug.Log(msg.ToString());
        }

        #endregion
    }
}
