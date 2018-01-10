using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyAIFrame;
using UnityEngine.UI;

public class TanksManager : MonoBehaviour {

    public float maxSpawnX;
    public float minSpawnX;
    public float maxSpawZ;
    public float minSpawZ;

    public Tank PrototypeTank;

    public Slider Slider;
    public Text Times;

    private List<Entity> tanks = new List<Entity>();

    void Start ()
    {
        Slider.maxValue = 1;
        PrototypeTank.gameObject.SetActive(false);
        EvolutionManager.Instance.OnGeneticRestartAction = resetTanks;
        EvolutionManager.Instance.OnGeneticUpdateAction = updateSlider;
    }

    void updateSlider(float _val)
    {
        Slider.value = _val;
    }

    bool hasBegin = false;
    private void OnGUI()
    {
        if (!hasBegin) {
            if (GUILayout.Button("启动遗传进程"))
            {
                hasBegin = true;
                begin();
            }
        }
    }

    void begin()
    {
        //获取种群数量
        int targetLength = EvolutionManager.Instance.PopulationSize;
        //生成相应数量的种群，生成后请勿删除种群单位
        for (int i = 0; i < targetLength; i++)
        {
            GameObject carCopy = Instantiate(PrototypeTank.gameObject);
            carCopy.transform.parent = transform;
            Entity controllerCopy = carCopy.GetComponent<Entity>();
            randomSpawn(carCopy.transform);
            tanks.Add(controllerCopy);
            carCopy.SetActive(true);
        }
        //将种群几何传递给遗传管理类，并开始遗传进程
        EvolutionManager.Instance.StartGenetic(tanks);
    }

    void resetTanks(uint _count)
    {
        Times.text = _count + "";
        for (int i = 0; i < tanks.Count; i++)
        {
            randomSpawn(tanks[i].transform);
        }
    }

    /// <summary>
    /// 在一定范围内随机位置
    /// </summary>
    /// <param name="tr"></param>
    void randomSpawn(Transform tr)
    {
        float x = Random.Range(minSpawnX, maxSpawnX);
        float z = Random.Range(minSpawZ, maxSpawZ);
        tr.position = new Vector3(x, PrototypeTank.transform.position.y, z);
    }
}
