using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MazeModelAgent: Maze_Agent
{
    
    
    public List<Vector2Int> startPointList;
    int startPointIndex;
    Vector2Int startPoint;
    
    
    public override void Initialize()
    {

        
        
        startPointIndex = -1;
        goal = new Vector2Int(stageCtrl.width - 1, stageCtrl.height - 1);


    }

    public override void OnEpisodeBegin()
    {
        startPointIndex++;
        startPoint = startPointList[startPointIndex % startPointList.Count];
        Debug.Log(startPoint);
        this.transform.position = getPos(startPoint);
        curPos = startPoint;
        minDis = Vector2Int.Distance(curPos, goal);
        stageCtrl.mapReset();
    }


    

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        int contorlSignal = actions.DiscreteActions[0];
        Vector2Int newPos = curPos + getActionVector(contorlSignal);
        
        if (newPos.y < 0 || newPos.y > stageCtrl.height - 1 || newPos.x < 0 || newPos.x > stageCtrl.width - 1) AddReward(-10);
        else {

            switch (stageCtrl.map[newPos.x, newPos.y].x) { 
                case 0:
                    curPos = newPos;
                    break;
                case 255://wall
                    AddReward(-10);
                    break;
               
                case 5://goal
                    AddReward(500);
                    curPos = newPos;
                    
                    EndEpisode();
                    break;


            }
            this.transform.position = getPos(curPos);
            AddReward(-0.1f);
        }
        
        float currDis = Vector2Int.Distance(curPos, goal);
        if (currDis < minDis)
        {
            AddReward(20f);
            minDis = currDis;
        }



        






    }

    


}
