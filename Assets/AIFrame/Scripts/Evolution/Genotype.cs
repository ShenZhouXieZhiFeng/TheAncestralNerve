using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

namespace AIFrame
{
    /// <summary>
    /// 基因型
    /// </summary>
    public class Genotype : IComparable<Genotype>, IEnumerable<float>
    {
        #region 属性

        /// <summary>
        /// 评分,本身
        /// </summary>
        public float Evaluation
        {
            get;
            set;
        }

        /// <summary>
        /// 适应性，相对于整个种群
        /// </summary>
        public float Fitness
        {
            get;
            set;
        }

        /// <summary>
        /// 基因型
        /// </summary>
        public float[] parameters;

        /// <summary>
        /// 基因型长度
        /// </summary>
        public int ParameterCount
        {
            get
            {
                return parameters == null ? 0 : parameters.Length;
            }
        }

        public float this[int index]
        {
            get
            {
                return parameters[index];
            }
            set
            {
                parameters[index] = value;
            }
        }
        #endregion

        #region 方法

        public Genotype(float[] pars)
        {
            parameters = pars;
            Fitness = 0;
        }

        /// <summary>
        /// 设置随机基因型
        /// </summary>
        /// <param name="minVal"></param>
        /// <param name="maxVal"></param>
        public void SetRandomParameters(float _minVal, float _maxVal)
        {
            if (_minVal > _maxVal) throw new ArgumentException("Minimum value > maximum value.");
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = Random.Range(_minVal, _maxVal);
            }
        }

        /// <summary>
        /// 拷贝返回基因型
        /// </summary>
        /// <returns></returns>
        public float[] GetParameterCopy()
        {
            float[] copy = new float[ParameterCount];
            for (int i = 0; i < ParameterCount; i++)
            {
                copy[i] = parameters[i];
            }
            return copy;
        }

        public int CompareTo(Genotype _other)
        {
            //降序排列
            return _other.Fitness.CompareTo(Fitness);
        }

        public IEnumerator<float> GetEnumerator()
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                yield return parameters[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                yield return parameters[i];
            }
        }
        #endregion
    }
}
