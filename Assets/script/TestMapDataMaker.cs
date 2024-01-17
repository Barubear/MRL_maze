using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TestMapDataMaker : MonoBehaviour
{
    public stageCtrl stageCtrl;
    int times;
    string log = ".\\Assets\\LogData\\TestFoodData.txt";
    // Start is called before the first frame update
    void Start()
    {
        times = 0;
        string[] mapDataStr = File.ReadAllLines(".\\Assets\\script\\mapData.txt");
        string mapData = mapDataStr[stageCtrl.mapDataIndex];
        stageCtrl.creatWall(mapData);
    }

    // Update is called once per frame
    void Update()
    {
        if (times < 1000) {


            stageCtrl.mapResetForTest();
            List<Vector2Int> data = stageCtrl.creatItemMapData(stageCtrl.foodNums, 0.85f, 12, stageCtrl.width - 1, 0, stageCtrl.height - 12);
            foreach (var item in data)
            {
                File.AppendAllText(log, item.x+","+item.y+" ");
            }
            File.AppendAllText(log,"\n");
            times++;
        }
        else
        {
            UnityEditor.EditorApplication.isPlaying = false;
        } 
    }

    private int GenerateRandomNumber(int min, int max, int more_possble_min, int more_possble_max, float _seed = 0.5f)
    {
        float seed = _seed;
        if (seed >= 1 || seed <= 0)
        {
            seed = 0.5f;
        }

        int randomNumber = Random.Range(min, max);


        if (randomNumber < more_possble_min || randomNumber > more_possble_max)
        {

            float additionalProbability = Random.Range(0f, 1f);
            if (additionalProbability < seed)
            {
                randomNumber = GenerateRandomNumber(min, max, more_possble_min, more_possble_max, seed);
            }
        }


        return randomNumber;
    }
}
