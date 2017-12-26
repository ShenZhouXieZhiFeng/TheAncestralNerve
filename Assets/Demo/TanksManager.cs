using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIFrame;

public class TanksManager : MonoBehaviour {

    public float maxSpawnX;
    public float minSpawnX;
    public float maxSpawZ;
    public float minSpawZ;

    public Tank PrototypeTank;

    private List<Entity> tanks = new List<Entity>();

    void Start ()
    {
        PrototypeTank.gameObject.SetActive(false);
        EvolutionManager.Instance.OnGeneticRestartAction = resetTanks;
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
        int targetLength = EvolutionManager.Instance.PopulationSize;
        for (int i = 0; i < targetLength; i++)
        {
            GameObject carCopy = Instantiate(PrototypeTank.gameObject);
            carCopy.transform.parent = transform;
            Entity controllerCopy = carCopy.GetComponent<Entity>();
            randomSpawn(carCopy.transform);
            tanks.Add(controllerCopy);
            carCopy.SetActive(true);
        }
        EvolutionManager.Instance.StartGenetic(tanks);
    }

    void resetTanks()
    {
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
