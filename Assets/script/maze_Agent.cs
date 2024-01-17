using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using UnityEngine;

public class Maze_Agent : Agent
{
    public stageCtrl stageCtrl;

    
    protected float minDis;
    public Vector2Int goal;
    public int foodnum;
    public int bobmnum;
    public Vector2Int curPos;
    public int stepNum = 0;
    int testNum = 0;
    public string AgentType;
    protected TestTool testTool;
    public int testTimes;
    string[] mapDataStr;
    string mapData;

    public override void Initialize() {

        mapDataStr = File.ReadAllLines(".\\Assets\\LogData\\TestFoodData.txt");




    }
    public override void OnEpisodeBegin()
    {
        if (testNum < testTimes)
        {
            if (testNum == 0) testTool.doLog("start");
            testTool.doLog("\n" + "#");
            stepNum = 0;
            mapData = mapDataStr[testNum];
            testNum++;
        }
        else {

            UnityEditor.EditorApplication.isPlaying = false;
        }
        
    }
    
    public void EndEpisodeForTest() {

        testTool.doLog("stepNum:"+ stepNum + ",foodNum:" + foodnum);
        EndEpisode();


    }
    protected Vector3 getPos(Vector2Int alex)
    {
        return new Vector3(stageCtrl.transform.position.x + 1.5f * alex.x, 0.5f, stageCtrl.transform.position.z - (1.5f * alex.y));

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*var aout = actionsOut.DiscreteActions;

        if (Input.GetKeyDown(KeyCode.A)) aout[0] = 2;
        else if (Input.GetKeyDown(KeyCode.D)) aout[0] = 1;
        else if (Input.GetKeyDown(KeyCode.W)) aout[0] = 4;
        else if (Input.GetKeyDown(KeyCode.S)) aout[0] = 3;
        else aout[0] = 0;*/
    }


    public Vector2Int getActionVector(int contorlSignal) {

        Vector2Int vector2 = Vector2Int.zero;
        switch (contorlSignal)
        {
            case 0:
                vector2 = Vector2Int.zero;
                break;
            case 1:
                vector2 = new Vector2Int(1, 0);

                break;
            case 2:
                vector2 = new Vector2Int(-1, 0);
                break;
            case 3:
                vector2 = new Vector2Int(0, -1);
                break;
            case 4:
                vector2 = new Vector2Int(0, 1);
                break;

        }
        return vector2;
    }

    public void creatItemForTest() {
        stageCtrl.creatGoal(new Vector2Int(34, 34));
        List<Vector2Int> map = new List<Vector2Int>();
        string[] data = mapData.Split(" ");
        foreach (var s in data) {
            if (s != "") {
                string[] strs = s.Split(",");
                map.Add(new Vector2Int( int.Parse(strs[0]), int.Parse(strs[1]) ) );
            }
        }
        stageCtrl.creatItemWithPosition(stageCtrl.food,2, map);

    }
}
