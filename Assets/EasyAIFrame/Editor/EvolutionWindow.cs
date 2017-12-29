using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyAIFrame
{
    public class EvolutionWindow : EditorWindow
    {
        static EvolutionWindow window;

        [MenuItem("EasyAIFrame/Editor")]
        static void Init()
        {
            window = (EvolutionWindow)GetWindow(typeof(EvolutionWindow), false, "遗传参数控制", true);
            window.Show();
        }

        private void OnGUI()
        {
            setSkin(GUI.skin);
            GameObject current = Selection.activeObject as GameObject;
            if (current == null)
            {
                GUILayout.Label("Selection is null");
                return;
            }
            EvolutionManager manager = current.GetComponent<EvolutionManager>();
            if (manager == null)
            {
                GUILayout.Label("Must Selection EvolutionManager GameObject");
                return;
            }
            showSetContent(manager);
        }

        void showSetContent(EvolutionManager _manager)
        {
            GUILayout.Space(10);
            GUILayout.Label("神经网络参数设定");
            GUILayout.Space(15);
            _manager.InputParameterCount = (uint)EditorGUILayout.IntField("输入参数个数:", (int)_manager.InputParameterCount);
            GUILayout.Space(10);
            _manager.OutPutParameterCount = (uint)EditorGUILayout.IntField("输出参数个数:", (int)_manager.OutPutParameterCount);
            GUILayout.Space(10);
            _manager.HiddenLayerCount = (uint)EditorGUILayout.IntField("隐藏层层数:", (int)_manager.HiddenLayerCount);
            GUILayout.Space(10);
            _manager.NeuronCountPerLayer = (uint)EditorGUILayout.IntField("每一层的神经节点数量:", (int)_manager.NeuronCountPerLayer);
            GUILayout.Space(10);
            GUILayout.Label("遗传普通参数设定");
            GUILayout.Space(15);
            _manager.PopulationSize = EditorGUILayout.IntField("种群数量:", _manager.PopulationSize);
            GUILayout.Space(10);
            _manager.GeneticCountlimit = EditorGUILayout.IntField("遗传次数上限:", _manager.GeneticCountlimit);
            GUILayout.Space(10);
            _manager.GeneticTimePerTimes = EditorGUILayout.FloatField("每次遗传的时间(s):", _manager.GeneticTimePerTimes);
            GUILayout.Space(15);
            GUILayout.Label("遗传高级参数设定");
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("基因交换概率:");
            GeneticAlgorithm.DefCrossSwapProb = EditorGUILayout.Slider(GeneticAlgorithm.DefCrossSwapProb, 0, 1);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("基因突变概率:");
            GeneticAlgorithm.DefMutationProb = EditorGUILayout.Slider(GeneticAlgorithm.DefMutationProb, 0, 1);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            
        }

        void setSkin(GUISkin skin)
        {
            skin.label.fontSize = 20;
            skin.label.normal.textColor = Color.gray;

            
        }
    }
}
