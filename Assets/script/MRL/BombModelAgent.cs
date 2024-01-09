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
    public int MaxBomb;
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
                   if(stageCtrl.map[newPos.x, newPos.y].y>3) AddReward(-1*stageCtrl.map[newPos.x, newPos.y].y);
                    stageCtrl.map[newPos.x, newPos.y].y++;
                    this.transform.position = getPos(curPos);
                    
                    break;
                case 255://wall
                    AddReward(-1);
                    break;
                case 3://bomb
                    this.transform.position = getPos(curPos);
                    stageCtrl.map[newPos.x, newPos.y].x = 0;
                    AddReward(-100);
                    
                    
                    bobmnum++;
                    stageCtrl.DestoryItem(newPos);

                    stageCtrl.map[newPos.x, newPos.y].y++;
                    break;
                case 5:
                    Debug.Log("completed");
                    AddReward(10* stageCtrl.boomNum - bobmnum);
                    EndEpisode();
                    break;
            }
            
        }
        if (bobmnum > MaxBomb) {
            AddReward(-1000);
            Debug.Log("bomb!");
            EndEpisode();
        }
    }


    public void IcreatItem()
    {
        stageCtrl.creatGoal(new Vector2Int(15, 5));
        
        
        int dic = Random.Range(0, 10);
            if (dic > 4)//up
            {
            
            stageCtrl.creatItem(stageCtrl.boomNum, stageCtrl.boom, 3, 0.85f, 0 , stageCtrl.width-1, 0, 6);
            }
            else { //down
            
            stageCtrl.creatItem(stageCtrl.boomNum, stageCtrl.boom, 3, 0.85f, 0, stageCtrl.width - 1, 7, stageCtrl.height-1);
            }

            
        

        
        //stageCtrl.creatItem(stageCtrl.boomNum / 4, stageCtrl.boom, 3);
        


    }
    public void RadomBomb()
    {
        stageCtrl.creatGoal(new Vector2Int(15, 5));
        for (int i = 0; i < stageCtrl.boomNum; i++) {
            int whileTimes = 0;
            while (whileTimes < 100)
            {

                Vector2Int newPos = new Vector2Int(Random.Range(0, stageCtrl.width), Random.Range(0, stageCtrl.height));
                
                if (newPos == startPoint) continue;
                if (stageCtrl.map[newPos.x, newPos.y].x != 0) continue;
                if (newPos.x + 1 < stageCtrl.width && stageCtrl.map[newPos.x + 1, newPos.y].x != 0) continue;
                if (newPos.x - 1 > 0 && stageCtrl.map[newPos.x - 1, newPos.y].x != 0) continue;
                if (newPos.y + 1 < stageCtrl.height && stageCtrl.map[newPos.x, newPos.y + 1].x != 0) continue;
                if (newPos.y - 1 > 0 && stageCtrl.map[newPos.x, newPos.y - 1].x != 0) continue;
                
                stageCtrl.creatItemWithPosition(stageCtrl.boom, 3, newPos);
                break;
                
                whileTimes++;
            }
        }
        

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
