using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BombModelAgent : Maze_Agent
{
    Vector2Int startPoint;
    
    public override void OnEpisodeBegin()
    {
        bobmnum = 0;
        startPoint = new Vector2Int(0, 5);
        this.transform.position = getPos(startPoint);
        stageCtrl.mapReset(IcreatItem);
        curPos = startPoint;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(bobmnum);
        sensor.AddObservation(bobmnum / stageCtrl.boomNum);
    }


    public override void OnActionReceived(ActionBuffers actions)
    {



        int contorlSignal = actions.DiscreteActions[0];


        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-1);
        else
        {

            switch (stageCtrl.map[newPos.x, newPos.y].x)
            {
                case 0:
                    curPos = newPos;
                    this.transform.position = getPos(curPos);
                    break;
                case 255://wall
                    AddReward(-1);
                    break;
                case 3://bomb
                    this.transform.position = getPos(curPos);
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    AddReward(-10* bobmnum);
                    Debug.Log("bomb!");
                    bobmnum++;
                    stageCtrl.DestoryItem(newPos);

                    //stageCtrl.map[newPos.x, newPos.y].y++;
                    break;
                case 5:
                    AddReward(100+100* (stageCtrl.boomNum - bobmnum));
                    EndEpisode();
                    break;
            }

        }
        AddReward(-0.01f);
    }


    public void IcreatItem()
    {
       int minX = 5, maxY = 10;
        for (int i = 0; i < 3; i++) { 
            int dic = Random.Range(0, 10);
            if (dic > 4)//up
            {
                stageCtrl.creatItemWithPosition(stageCtrl.boom, 3, creatRadomPointList(stageCtrl.boomNum / 4, minX, maxY, 0, 4));
            }
            else { //down

                stageCtrl.creatItemWithPosition(stageCtrl.boom, 3, creatRadomPointList(stageCtrl.boomNum / 4, minX, maxY, 6, 10));
            }

            minX += 12;
            maxY += 12;
        }

        Debug.Log("creat");
        stageCtrl.creatItem(stageCtrl.boomNum / 4, stageCtrl.boom, 3);
        stageCtrl.creatGoal(new Vector2Int(40, 5));


    }


    public List<Vector2Int> creatRadomPointList(int lenth , int minX, int maxX, int minY, int maxY) {

        
        List< Vector2Int > res= new List< Vector2Int >();
        for (int i = 0; i < lenth; i++) {
            while (true) {
                Vector2Int newPos = new Vector2Int(Random.Range(minX, maxX), Random.Range(minY, maxY));
                if (!res.Contains(newPos)) { 
                    res.Add(newPos);
                    
                    break;
                }
            }
        
        }

        return res;

    }
}
