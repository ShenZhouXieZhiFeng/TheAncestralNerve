using EasyAIFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesManager : SingletonMono<MinesManager> {

    public Transform Mines;
    public GameObject MinePrefab;
    public Vector4 SpawnDis;
    public int MaxMineNums = 20;

    private int currentMineNum = 0;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < MaxMineNums; i++) {
            spawnMine();
        }
	}
	
    //随机生成地雷
    void spawnMine() {
        float x = Random.Range(SpawnDis.x, SpawnDis.y);
        float z = Random.Range(SpawnDis.z, SpawnDis.w);
        GameObject newMine = Instantiate(MinePrefab);
        newMine.transform.parent = Mines;
        newMine.transform.position = new Vector3(x, 1, z);
        currentMineNum++;
    }

    public void checkMineNums() {
        currentMineNum--;
        if (currentMineNum < MaxMineNums - 5) {
            for (int i = 0; i < 5; i++) {
                spawnMine();
            }
        }
    }
}
