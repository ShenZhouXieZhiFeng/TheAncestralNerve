using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace EasyAIFrame
{
    [CustomEditor(typeof(EvolutionManager))]
    public class EvolutionEditor : Editor
    {
        EvolutionManager _this;

        private void OnEnable()
        {
            _this = (EvolutionManager)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("种群数量:");
            EditorGUILayout.LabelField(_this.PopulationSize.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("遗传次数上限:");
            EditorGUILayout.LabelField(_this.GeneticCountlimit.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("每次遗传的时间(s):");
            EditorGUILayout.LabelField(_this.GeneticTimePerTimes.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("本次遗传进度(S):");
            EditorGUILayout.LabelField(_this.currentTime.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("当前遗传次数:");
            EditorGUILayout.LabelField(_this.GenerationCount.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("基因交换概率:");
            EditorGUILayout.LabelField(GeneticAlgorithm.DefCrossSwapProb.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("基因突变概率:");
            EditorGUILayout.LabelField(GeneticAlgorithm.DefMutationProb.ToString());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
    }
}
